using System;

namespace Common.Helpers
{
    public class UrlHelper
    {
        public static string GetRootUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                return string.Format("{0}://{1}", uri.Scheme, uri.Authority);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
