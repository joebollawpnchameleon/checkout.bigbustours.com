using System;
using bigbus.checkout.data.Model;
using Common.Model;

namespace Services.Infrastructure
{
    public interface IAuthenticationService
    {
        string GetExternalSessionId(string sessionCookieName);

        Guid GetSessionId(string sessionCookieName);

        Guid CreateNewSession(Guid basketId, Guid currencyId, string sessionCookieDomain, string sessionCookieName);

        CustomerSession PutSessionInCheckoutMode(string sessionId);

        void MoveSessionOutOfCheckoutMode(string sessionId);

        Session PutSessionInOrderCreationMode(Guid sessionId);

        string GetBasketIdFromCookie(string basketCookieName);

        Session GetSession(Guid sessionId);

        void PutSessionInOrderCreationMode(Session session);

        void UpdateSession(Session session);

        bool ExpireCookie(string cookieName);

        bool SetCookie(string cookieName, string domain, string value);

    }
}
