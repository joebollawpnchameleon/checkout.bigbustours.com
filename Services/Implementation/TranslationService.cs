
using System.Collections.Generic;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Services.Infrastructure;

namespace Services.Implementation
{
    public class TranslationService : ITranslationService
    {
        private readonly IGenericDataRepository<Language> _languageRepository;
        private readonly IGenericDataRepository<Phrase> _phraseRepository;
        private readonly IGenericDataRepository<PhraseLanguage> _phraseLanguageRepository;
        private readonly string _defaultLanguageId;

        public TranslationService(IGenericDataRepository<Language> languageRepository,
            IGenericDataRepository<Phrase> phraseRepository,
            IGenericDataRepository<PhraseLanguage> phraseLanguageRepository,
            string defaultLanguageId)
        {
            _languageRepository = languageRepository;
            _phraseRepository = phraseRepository;
            _phraseLanguageRepository = phraseLanguageRepository;
            _defaultLanguageId = defaultLanguageId;
        }

        public List<Language> GetAllLanguages()
        {
            return (List<Language>) _languageRepository.GetAll();
        } 

        public string TranslateTerm(string key, string language)
        {
            //***get from cache

            //check original translation in DB
            var translation = _phraseLanguageRepository.GetSingle(x => !string.IsNullOrEmpty(x.PhraseId) &&
                                                                       x.PhraseId.Equals(key, System.StringComparison.CurrentCultureIgnoreCase)
                                                                       && !string.IsNullOrEmpty(x.LanguageId) &&
                                                                       x.LanguageId.Equals(language, System.StringComparison.CurrentCultureIgnoreCase));

            //if translation not found in that language, lets check default language in case they are different
            if(translation == null && !language.Equals(_defaultLanguageId, System.StringComparison.CurrentCultureIgnoreCase))
            {
               translation = _phraseLanguageRepository.GetSingle(x => !string.IsNullOrEmpty(x.PhraseId) &&
                                                                      x.PhraseId.Equals(key, System.StringComparison.CurrentCultureIgnoreCase)
                                                                      && !string.IsNullOrEmpty(x.LanguageId) &&
                                                                      x.LanguageId.Equals(_defaultLanguageId, System.StringComparison.CurrentCultureIgnoreCase));

            }

            return (translation != null)? translation.Translation : key;
        }
       
    }
}
