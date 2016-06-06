
using System;
using System.Web;
using System.Web.UI;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Common.Enums;
using Common.Model;
using Services.Infrastructure;

namespace Services.Implementation
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly IGenericDataRepository<Session> _sessionRepository;

        public bool ExpireCookie(string cookieName)
        {
            var oldCookie = HttpContext.Current.Request.Cookies[cookieName];

            if (oldCookie == null) return true;

            oldCookie.Expires = DateTime.Now.Add(new TimeSpan(-1, 0, 0, 0));
            HttpContext.Current.Response.Cookies.Add(oldCookie);

            return true;
        }

        public bool SetCookie(string cookieName, string domain, string value)
        {
            ExpireCookie(cookieName);

            var cookie = new HttpCookie(cookieName, value)
            {
                Domain = domain,
                HttpOnly = true
            };

            HttpContext.Current.Response.Cookies.Add(cookie);   
            return true;
        }

        public AuthenticationService(IGenericDataRepository<Session> sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public virtual string GetExternalSessionId(string sessionCookieName)
        {
            var sessCookie = HttpContext.Current.Request.Cookies[sessionCookieName];
            return sessCookie == null ? string.Empty : sessCookie.Value;
        }
        
        public virtual Guid GetSessionId(string sessionCookieName)
        {
            var sessCookie = HttpContext.Current.Request.Cookies[sessionCookieName];
            return sessCookie == null ? new Guid() : new Guid(sessCookie.Value);
        }

        public static string GetCookieValue(string cookieName)
        {
            try
            {
                var cookie = HttpContext.Current.Request.Cookies[cookieName];
                if (cookie != null) return cookie.Value;
            }
            catch 
            {
                //ignore
            }
            return string.Empty;
        }

        public virtual Session GetSession(Guid sessionId)
        {
            return _sessionRepository.GetSingle(x => x.Id == sessionId);
        }

        public virtual Guid CreateNewSession(Guid basketId, Guid currencyId, string sessionCookieDomain, string sessionCookieName)
        {
            var newSession = new Session
            {
                BasketId = basketId,
                DateCreated = DateTime.Now,
                CurrencyId = currencyId.ToString(),
                DateModified = DateTime.Now
            };

            _sessionRepository.Add(newSession);

            SetCookie(sessionCookieName, sessionCookieDomain, newSession.Id.ToString());

            return newSession.Id;
        }

        public virtual CustomerSession PutSessionInCheckoutMode(string sessionId)
        {
            var session = _sessionRepository.GetSingle(x => x.Id == new Guid(sessionId));

            if (session == null) return null;

            session.InCheckoutProccess = true;
            _sessionRepository.Update(session);
            
            return new CustomerSession
            {
                BasketId = session.BasketId ?? Guid.Empty, 
                DbId = session.Id, 
                CurrencyId = new Guid(session.CurrencyId)
            };
        }

        public virtual void MoveSessionOutOfCheckoutMode(string sessionId)
        {
            var session = _sessionRepository.GetSingle(x => x.Id == new Guid(sessionId));

            if (session == null) return;

            session.InCheckoutProccess = false;
            _sessionRepository.Update(session);
        }

        public virtual Session PutSessionInOrderCreationMode(Guid sessionId)
        {
            var session = _sessionRepository.GetSingle(x => x.Id == sessionId);

            if (session == null)
                return null;
            
            session.InCheckoutProccess = false;
            session.InOrderCreationProcess = true;
            _sessionRepository.Update(session);

            return session;
        }

        public virtual void PutSessionInOrderCreationMode(Session session)
        {
            session.InCheckoutProccess = false;
            session.InOrderCreationProcess = true;
            _sessionRepository.Update(session);
        }

        public virtual string GetBasketIdFromCookie(string basketCookieName)
        {
            var basketCookie = HttpContext.Current.Request.Cookies[basketCookieName];
            return basketCookie == null ? string.Empty : basketCookie.Value;
        }

        public virtual void UpdateSession(Session session)
        {
            _sessionRepository.Update(session);
        }
       
    }
}
