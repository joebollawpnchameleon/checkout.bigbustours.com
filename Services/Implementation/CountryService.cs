using System.Collections.Generic;
using System.Linq;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Services.Infrastructure;

namespace Services.Implementation
{
    public class CountryService : ICountryService
    {
        private readonly IGenericDataRepository<Country> _countryRepository;
        private readonly ITranslationService _translationService;

        public CountryService(IGenericDataRepository<Country> countryRepository, ITranslationService translationService)
        {
            _countryRepository = countryRepository;
            _translationService = translationService;
        }

        public List<KeyValuePair<string, string>> GetAllCountriesWithNewOnTop(string lstCountriesOnTopCodes, string languageId)
        {
            var allCountries = _countryRepository.GetAll();
            
            if (!allCountries.Any())
            {
                //log and return 
                return null;
            }

            var returnList = new List<KeyValuePair<string, string>>();
            var topCountryCodes = lstCountriesOnTopCodes.Split(',').Select(x => x.ToLower());
            var tempListCountries = allCountries.Where(c => topCountryCodes.Contains(c.Id.ToLower()));

            var listCountries = tempListCountries as IList<Country> ?? tempListCountries.ToList();

            returnList.Add(new KeyValuePair<string, string>("-", 
                _translationService.TranslateTerm("Booking_SelectCountry", languageId)));

            if (listCountries.Any())
            {
                returnList.AddRange(listCountries.Select(kvp => new KeyValuePair<string, string>(kvp.Id,kvp.Name)));
            }

            returnList.Add(new KeyValuePair<string, string>("--","============================"));

            returnList.AddRange(allCountries.OrderBy(x => x.Name) 
                .Select(kvp => new KeyValuePair<string, string>(kvp.Id,kvp.Name)));

            return returnList;
        }
    }
}
