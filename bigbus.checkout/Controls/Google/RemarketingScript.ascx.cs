
namespace bigbus.checkout.Controls.Google
{
    public partial class RemarketingScript : BaseControl
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!string.IsNullOrWhiteSpace(BasePage.CurrentSite.GoogleRemarketingConversionId) &&
                !string.IsNullOrWhiteSpace(BasePage.CurrentSite.GoogleRemarketingConversionLabel))
            {
                Visible = true;
            }
            else
            {
                Visible = true;
            }
        }
    }
}
