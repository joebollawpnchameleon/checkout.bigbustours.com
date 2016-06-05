

using bigbus.checkout.Controls;
using bigbus.checkout.data.Model;
using BigBusWebsite.controls.SharedLayout;

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
            Navigation navigation = BasePage.GetFooterNavigation();

            if (navigation != null)
            {
                var items = navigation.NavigationItems;

                if (items != null && items.Count > 0)
                {
                    rptFooterNavigation.DataSource = BasePage.GetTranslatedItems(items);
                    rptFooterNavigation.DataBind();
                    rptFooterNavigation.Visible = true;
                }
            }
            else
            {
                rptFooterNavigation.Visible = false;
            }
        }
    }
}
