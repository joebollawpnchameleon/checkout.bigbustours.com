using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Helpers
{
    public class Affiliates
    {
        private const string AffiliateRequestName = "AffiliateRequestName";
        private const string AffiliateCookieExpiration = "AffiliateCookieExpiration";
        private const string AffiliateCookieName = "AffiliateCookieName";
        private const string AffiliateCookieKey = "AffiliateCookieKey";

        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }

        /// <summary>
        /// Return null if no source is present
        /// </summary>
        /// <returns></returns>
        public string GetAffilliateNetworkSource()
        {
            var affilliateCookieName = ConfigurationManager.AppSettings[AffiliateCookieName].Trim();
            if (string.IsNullOrWhiteSpace(affilliateCookieName)) affilliateCookieName = "_afco";

            var affilliateCookieKey = ConfigurationManager.AppSettings[AffiliateCookieKey].Trim();
            if (string.IsNullOrWhiteSpace(affilliateCookieKey)) affilliateCookieKey = "source";

            // do we have a cookie
            var cookies = GetAllCookies(affilliateCookieName);

            var sourceCookie = cookies.OrderBy(c => c.Domain).LastOrDefault();
            if (sourceCookie == null) return null;

            // do we have a source
            var source = sourceCookie[affilliateCookieKey];

            return source;
        }

        private IEnumerable<HttpCookie> GetAllCookies(string cookiName)
        {
            var cookies = new List<HttpCookie>();
            var count = Request.Cookies.Count;
            for (var i = 0; i < count; i++)
            {
                var cookie = Request.Cookies[i];
                if (cookie == null) continue;
                if (cookie.Name.Equals(cookiName, StringComparison.CurrentCultureIgnoreCase))
                    cookies.Add(cookie);
            }
            return cookies;
        }

        public void UpdateAffiliateNetworkSource()
        {
            // setup
            var affiliateRequestName = ConfigurationManager.AppSettings[AffiliateRequestName].Trim();
            if (string.IsNullOrWhiteSpace(affiliateRequestName)) affiliateRequestName = "source";

            int expires;
            var affiliateCookieExpiration = ConfigurationManager.AppSettings[AffiliateCookieExpiration].Trim();
            if (!int.TryParse(affiliateCookieExpiration, out expires)) expires = 999;

            var affilliateCookieName = ConfigurationManager.AppSettings[AffiliateCookieName].Trim();
            if (string.IsNullOrWhiteSpace(affilliateCookieName)) affilliateCookieName = "_afco";

            var affilliateCookieKey = ConfigurationManager.AppSettings[AffiliateCookieKey].Trim();
            if (string.IsNullOrWhiteSpace(affilliateCookieKey)) affilliateCookieKey = "source";

            // is their a source setting in the request
            var source = Request[affiliateRequestName];
            if (source == null) return;

            // do we have a cookie
            var cookie = new HttpCookie(affilliateCookieName)
            {
                Domain = ConfigurationManager.AppSettings["Session.CookieDomain"].Trim(),
                Expires = DateTime.UtcNow.AddDays(expires),
            };
            cookie[affilliateCookieKey] = source;
            Response.Cookies.Add(cookie);
        }
    }
}
