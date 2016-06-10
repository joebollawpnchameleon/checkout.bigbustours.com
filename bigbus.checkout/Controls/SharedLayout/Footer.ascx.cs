

using System.Collections.Generic;
using System.Configuration;
using bigbus.checkout.Controls;
using bigbus.checkout.data.Model;
using BigBusWebsite.controls.SharedLayout;
using Common.Model;

namespace bigbus.checkout.Controls.SharedLayout
{
    public partial class Footer : BaseControl
    {
        public  void Page_PreRender(object o, System.EventArgs a)
        {
            LoadFooter();
        }

        //This method is duplicated so any changes here need to be made there as well
        private void LoadFooter()
        {
            //var navigationItems = BasePage.GetFooterNavigation();

            //if (navigationItems == null || navigationItems.Count < 1)
            //    return;

            //*** format url in front page to go to born

            rptFooterNavigation.DataSource = LoadFooterLinks();
            rptFooterNavigation.DataBind();
            rptFooterNavigation.Visible = true;
        }

        public List<FrontEndNavigationItem> LoadFooterLinks()
        {
            return new List<FrontEndNavigationItem>
            {
                new FrontEndNavigationItem
                {
                    Text = BasePage.GetTranslation("TermsAndConditions"),
                    Url = ConfigurationManager.AppSettings["TermAndCo.Url"]
                },
                new FrontEndNavigationItem
                {
                    Text = BasePage.GetTranslation("PrivacyPolicyandCookie"),
                    Url = ConfigurationManager.AppSettings["Privacy.Url"]
                }
            };
        }
    }
}
