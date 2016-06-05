using System;

namespace bigbus.checkout.Controls.ViatorWidget
{
    public partial class Mobile : BaseControl
    {
        public void Page_Load(object o, EventArgs a)
        {
            if (BasePage.CurrentSite.ViatorWidgetDestinationId == null)
            {
                Visible = false;
            }
        }
    }
}
