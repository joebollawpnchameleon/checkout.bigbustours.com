using bigbus.checkout.Controls;
using System;
using System.Linq;

namespace bigbus.checkout.Controls.SurveyMonkey
{
    public partial class Survey : BaseControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var subsite = BasePage.MicrositeId;
            var navigateUrl = GetNavigateUrl(subsite);

            if (!string.IsNullOrWhiteSpace(navigateUrl))
                SurveyUrl = navigateUrl;

            if (USCitiesOnly)
            {
                var surveysites = new[] { "lasvegas", "newyork", "sanfrancisco", "miami", "washington" };
                Visible = surveysites.Any(x => x == subsite);
            }
        }

        private static string GetNavigateUrl(string subsite)
        {
            string navigateUrl;

            switch (subsite)
            {
                case "lasvegas":
                {
                    navigateUrl = "https://www.surveymonkey.com/s/las-vegas-pre";
                    break;
                }

                case "newyork":
                {
                    navigateUrl = "https://www.surveymonkey.com/s/new-york-pre";
                    break;
                }

                case "sanfrancisco":
                {
                    navigateUrl = "https://www.surveymonkey.com/s/san-fran-pre";
                    break;
                }

                case "miami":
                {
                    navigateUrl = "https://www.surveymonkey.com/s/miami-pre";
                    break;
                }

                case "washington":
                {
                    navigateUrl = "https://www.surveymonkey.com/s/washington-dc-pre";
                    break;
                }

                default:
                {
                    return null;
                }
            }

            return navigateUrl;
        }

        public bool USCitiesOnly { get; set; }

        public string SurveyUrl { get; private set; }

        public string MicroSite { get; set; }
       
    }
}