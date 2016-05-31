using Services.Implementation;
using Services.Infrastructure;
using System.ComponentModel.DataAnnotations;
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
            //get language
            var term = _translationService.TranslateTerm(_name, "eng");
            ErrorMessage = term;
            return base.FormatErrorMessage(term);
        }
    }
}
