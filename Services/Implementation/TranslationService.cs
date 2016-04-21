
using System.Collections.Generic;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Services.Infrastructure;

namespace Services.Implementation
{
    public class TranslationService : ITranslationService
    {
        private readonly IGenericDataRepository<Language> _languageRepository;

        public TranslationService(IGenericDataRepository<Language> languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public List<Language> GetAllLanguages()
        {
            return (List<Language>) _languageRepository.GetAll();
        } 

        public string TranslateTerm(string key, string language)
        {
            //get from cache
            return key;
        }
       
    }
}
