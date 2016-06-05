using Common.Helpers.TrustPilot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.Caching;
using System.Web.Script.Serialization;
using bigbus.checkout.Models;

namespace bigbus.checkout.Controls
{
    public class BaseControl : System.Web.UI.UserControl
    {
        public FeedModel GetTrustPilotFeed()
        {
            // Check if we have a feed in the cache
            var cacheKey = "BigBus.TrustPilot.Feed" + BasePage.CurrentSite.Id;

            var cachedFeed = Cache.Get(cacheKey) as FeedModel;

            if (cachedFeed != null)
            {
                return cachedFeed;
            }

            // Make webrequest
            // You url can be found on http://b2b.trustpilot.com/Modules/Plugins, where you also find the documentation.

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(BasePage.CurrentSite.TrustPilotUrl);
                request.Method = WebRequestMethods.Http.Get;
                request.AutomaticDecompression = DecompressionMethods.GZip; // Remember to unzip the feed

                // Download the json data
                string feedData = null;

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            feedData = new StreamReader(responseStream).ReadToEnd();
                        }
                    }
                }

                // Deserialize the json
                if (feedData != null)
                {
                    var feed = new JavaScriptSerializer().Deserialize<FeedModel>(feedData);

                    // Add to cache
                    Cache.Add(cacheKey, feed, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 1, 0), CacheItemPriority.Normal, null);

                    // Return the feed
                    return feed;
                }
            }
            catch (Exception)
            {
            }

            return new FeedModel();
        }

        public string ShortenTrustPilotText(string original, int maxLength)
        {
            var htmlDecoded = Server.HtmlDecode(original);

            return
                htmlDecoded.Length > maxLength ?
                    string.Format("{0}…", Server.HtmlEncode(htmlDecoded.Substring(0, maxLength - 1)))
                    : original;
        }

        public virtual BasePage BasePage
        {
            get
            {
                return Page as BasePage;
            }
        }

        //public string SiteUrl(string pagename)
        //{
        //    return BasePage.SiteUrl(pagename);
        //}
    }
}