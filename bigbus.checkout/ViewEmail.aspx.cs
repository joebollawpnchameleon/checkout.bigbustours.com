using bigbus.checkout.Models;
using System;

namespace bigbus.checkout
{
    public partial class ViewEmail : BasePage
    {
        public string EmailDump { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            var orderId = Request.QueryString["oid"];

            if (string.IsNullOrEmpty(orderId))
                return;

            LoadEmail(orderId);
        }

        private void LoadEmail(string orderId)
        {
            try
            {
                var email = NotificationService.GetEmailByOrderId(orderId);
                EmailDump = email.HTMLBody;
            }
            catch
            {
                EmailDump = @"<span style=""color:red;"">" + GetTranslation("EmailLoadError") + "</span>";
            }
        }
    }
}