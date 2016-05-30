using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bigbus.checkout.data.Model;
using Common.Model.Interfaces;
using Services.Infrastructure;

namespace bigbus.checkout.Helpers
{
    public class LanguageHelper
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ITranslationService _translationService;

        public string LanguageCacheKey
        {
            get
            {
                return "Languages_All";
            }
        }

        public LanguageHelper(ICacheProvider cacheProvider, ITranslationService translationService)
        {
            _cacheProvider = cacheProvider;
            _translationService = translationService;
        }

        /// <summary>
        /// Returns key,value list as LanguageFullname, LanguageId
        /// </summary>
        /// <returns></returns>
        public List<Language> GetLanguages()
        {
            var languages = _cacheProvider.GetFromCache<List<Language>>(LanguageCacheKey);

            if (languages != null) return languages;

            languages = new List<Language>();

            GetLanguageList(languages, LanguageCacheKey);

            return languages;
        }

        private void GetLanguageList(List<Language> cachedLangs, string languageCachKey)
        {
            var langs = _translationService.GetAllLanguages();

            cachedLangs.AddRange(langs);

            //cache this for 5 mins
            if (cachedLangs.Count > 0)
            {
                _cacheProvider.AddToCache(languageCachKey, cachedLangs, DateTime.Now.AddMinutes(5));
            }
        }

        public bool HasValidLanguageHostPart(string hostName)
        {
            // return if a supported language host part is present
            return GetLanguages().Any(rh => string.Equals(this.GetLanguageHostPart(hostName), rh.Id.ToLower()));
        }

        public bool HasWwwHostPart(string hostName)
        {
            // return if a supported language host part is present
            return this.GetLanguageHostPart(hostName).Equals("WWW", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// returns the language of the context.request URL domain
        /// </summary>
        /// <returns></returns>
        public Language GetLanguageFromRequestDomain(string hostName)
        {
            var languages = GetLanguages();

            // return if a supported language host part is present
            var language = languages.FirstOrDefault(rh => string.Equals(this.GetLanguageHostPart(hostName), rh.Id.ToLower()));

            return language;
        }

        private string GetLanguageHostPart(string hostName)
        {
            string subdomain = hostName;

            if (hostName.Contains('.'))
            {
                subdomain = hostName.Substring(0, hostName.IndexOf('.'));
            }

            return subdomain;
        }
    }
}