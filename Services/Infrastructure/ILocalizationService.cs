

using System;

namespace Services.Infrastructure
{
    public interface ILocalizationService
    {
        string GetCityTime(string microSiteId);

        DateTime GetLocalDateTime(string microSiteId);

        string GetCityTimeZoneStandardName(string microSiteId);
    }
}
