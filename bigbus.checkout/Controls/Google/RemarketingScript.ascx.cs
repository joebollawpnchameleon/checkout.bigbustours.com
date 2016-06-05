

using bigbus.checkout.Controls;

namespace BigBusWebsite.controls.Google
{
    public partial class RemarketingScript : BaseControl
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!string.IsNullOrWhiteSpace(BasePage.CurrentSite.GoogleRemarketingConversionId) &&
                !string.IsNullOrWhiteSpace(BasePage.CurrentSite.GoogleRemarketingConversionLabel))
            {
                this.Visible = true;
            }
            else
            {
                this.Visible = true;
            }
        }
    }
}
