using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bigbus.checkout.mvc.Controllers
{
    public class BaseController : Controller
    {
        public IAuthenticationService AuthenticationService { get; set; }
        public ISiteService SiteService { get; set; }
        public ILoggerService LoggerService { get; set; }
        public ITranslationService TranslationService { get; set; }
        public INotificationService NotificationService { get; set; }
        public ILocalizationService LocalizationService { get; set; }
        public IApiConnectorService ApiConnector { get; set; }

        public string ExternalBasketCookieName { get { return ConfigurationManager.AppSettings["External.Basket.CookieName"]; } }
        public string SessionCookieName { get { return ConfigurationManager.AppSettings["Session.CookieName"]; } }
        public string SessionCookieDomain { get { return ConfigurationManager.AppSettings["Session.CookieDomain"]; } }
        public string PciApiDomain { get { return ConfigurationManager.AppSettings["PciWebsite.ApiDomain"]; } }
        public string PciDomain { get { return ConfigurationManager.AppSettings["PciWebsite.Domain"]; } }
        public string PciLandingPagePath { get { return ConfigurationManager.AppSettings["PciWebsite.LandingPagePath"]; } }
        public string BasketCookieName { get { return ConfigurationManager.AppSettings["Basket.CookieName"]; } }
        public string GoogleChartUrl { get { return ConfigurationManager.AppSettings["GoogleChartUrl"]; } }
        public string LiveEcrEndPoint
        {
            get { return ConfigurationManager.AppSettings["LiveEcrEndPoint"]; }
        }
        public string EcrApiKey { get { return ConfigurationManager.AppSettings["EcrApiKey"]; } }
        public int EnvironmentId { get { return (ConfigurationManager.AppSettings["Environment"] != null) ? Convert.ToInt32(ConfigurationManager.AppSettings["Environment"]) : (int)Common.Enums.Environment.Local; } }

        protected string CurrentLanguageId { get { return "eng"; } } //***replace with function
        protected string MicrositeId { get { return "london"; } } //*** get from url as in function
        protected string SubSite { get { return "london"; } }
    }
}