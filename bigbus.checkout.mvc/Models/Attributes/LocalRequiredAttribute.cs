using Services.Implementation;
using Services.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Web.Mvc;

namespace Common.Model
{
    public class LocalRequiredAttribute : RequiredAttribute
    {
        private ITranslationService _translationService;
        private string _name;

        public LocalRequiredAttribute(string name)
        {
            _name = name;
            _translationService = DependencyResolver.Current.GetService<ITranslationService>();
        }

        public string Name{ get; set; }

        public override string FormatErrorMessage(string name)
        {
            var language = GetUserLanguage();
            var term = _translationService.TranslateTerm(_name, language);
            ErrorMessage = term;
            return base.FormatErrorMessage(term);
        }

        private string GetUserLanguage()
        {
            var langCookieName = ConfigurationManager.AppSettings["Session.LanguageCookieName"];
            var defaultLanguage = ConfigurationManager.AppSettings["Default.Language"];

            var userLanguage = AuthenticationService.GetCookieValue(langCookieName);

            return string.IsNullOrEmpty(userLanguage) ? defaultLanguage : userLanguage;
        }
    }
}
