

using System;
using System.Collections.Generic;
using System.Web.Helpers;
using Common.Model;
using Common.Model.Interfaces;
using Services.Infrastructure;

namespace Services.Implementation
{
    public class LocalizationService : ILocalizationService
    {
        private readonly ICacheProvider _cacheProvider;

        public LocalizationService(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }
        
        public string GetCityTime(string microSiteId)
        {
            return GetLocalDateTime(microSiteId).ToLongTimeString();
        }

        public DateTime GetLocalDateTime(string microSiteId)
        {
            var citydatetime = DateTime.Now;

            if (microSiteId == "london" || microSiteId == "international")
            {
                return DateTime.Now;
            }

            const string timeZonesCacheKey = "TimeZones";

            var timeZoneList = _cacheProvider.GetFromCache<Dictionary<string, TimeZoneInformation>>(timeZonesCacheKey);

            if (timeZoneList == null)
            {
                //read and populate time zones from the registry
                var mZones = TimeZoneInformation.EnumZones();
                Array.Sort(mZones, new TimeZoneComparer());

                timeZoneList = new Dictionary<string, TimeZoneInformation>();

                foreach (var tzone in mZones)
                {
                    if (!timeZoneList.ContainsKey(tzone.Name))
                        timeZoneList.Add(tzone.Name, tzone);
                }

                //cache this for 5 mins
                if (timeZoneList.Count > 0)
                {
                    _cacheProvider.AddToCache(timeZonesCacheKey, timeZoneList, DateTime.Now.AddDays(1));
                }
            }

            var tzsn = GetCityTimeZoneStandardName(microSiteId);

            if (!timeZoneList.ContainsKey(tzsn)) return citydatetime;

            var destinationTimeZoneInfo = timeZoneList[tzsn];
            var local = DateTime.Now;
            var utc = local.ToUniversalTime();

            if (destinationTimeZoneInfo == null) return citydatetime;

            var destinationTime = destinationTimeZoneInfo.FromUniversalTime(utc);
            citydatetime = destinationTime;

            return citydatetime;
        }

        public string GetCityTimeZoneStandardName(string microSiteId)
        {
            var standardName = string.Empty;

            switch (microSiteId)
            {
                // Asian-/Australian cities
                case "hongkong":
                case "shanghai":
                    standardName = "China Standard Time";
                    break; //"(UTC+08:00) Beijing, Chongqing, Hong Kong, Urumqi"

                case "sydney":
                    standardName = "AUS Eastern Standard Time";
                    break;

                // European cities
                case "istanbul":
                    standardName = "E. Europe Standard Time";
                    break; // +02:00

                case "budapest":
                case "paris":
                case "rome":
                case "vienna":
                    standardName = "Central European Standard Time";
                    break;

                // Middle east cities
                case "abudhabi":
                case "dubai":
                case "muscat":
                    standardName = "Arabian Standard Time";
                    break; //"(UTC+04:00) Abu Dhabi, Muscat"

                // North American cities
                case "chicago":
                    standardName = "Central Standard Time"; // "Central Time Zone";
                    break; //"(UTC-05:00) Eastern Time (US & Canada)"

                case "miami":
                case "newyork":
                case "philadelphia":
                case "washington":
                    standardName = "Eastern Standard Time";
                    break; //"(UTC-05:00) Eastern Time (US & Canada)"

                case "lasvegas":
                case "sanfrancisco":
                    standardName = "Pacific Standard Time";
                    break;

                // Language stuff that has no business being here but I'm not going to remove it
                case "rus":
                case "por":
                case "spain":
                    standardName = string.Empty;
                    break;
            }

            return standardName;
        }

        
    }
}
