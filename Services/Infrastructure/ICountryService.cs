

using System.Collections.Generic;

namespace Services.Infrastructure
{
    public interface ICountryService
    {
        List<KeyValuePair<string, string>> GetAllCountriesWithNewOnTop(string lstCountriesOnTopCodes, string languageId);
    }
}
