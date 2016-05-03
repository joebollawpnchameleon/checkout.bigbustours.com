

using System.Collections.Generic;
using bigbus.checkout.data.Model;

namespace Services.Infrastructure
{
    public interface ITranslationService
    {
        string TranslateTerm(string key, string language);

        List<Language> GetAllLanguages();

        Language GetLanguage(string id);

    }
}
