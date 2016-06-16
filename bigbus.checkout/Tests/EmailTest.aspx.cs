using System;
using System.Configuration;
using System.Drawing;
using bigbus.checkout.Models;
using Common.Model;
using Services.Implementation;
using Services.Infrastructure;

namespace bigbus.checkout.TestingBeforeLive
{
    public partial class EmailTestSend : BasePage
    {
        private IEmailService _emailService;

        public IEmailService EmailService
        {
            get
            {
                if (_emailService == null)
                {
                    var smtpSettings = new SmtpSettings
                    {
                        UserName = ConfigurationManager.AppSettings["SMTPUsername"],
                        Password = ConfigurationManager.AppSettings["SMTPPass"],
                        SMTPServer = ConfigurationManager.AppSettings["SMTPServerName"]
                    };

                    var mailPickupDir = ConfigurationManager.AppSettings["MailPickupDir"];

                    _emailService = new EmailService(smtpSettings, mailPickupDir);
                }

                return _emailService;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
       
        protected void SendTopEmailTest(object sender, EventArgs e)
        {
            var email = NotificationService.GetEmail("joe.bolla@wpnchameleon.co.uk");
            var result = EmailService.SendEmailHTML(email.ToAddresses, "Test User", email.FromAddress, "BB Admin Test",
                email.Subject, email.HTMLBody);

            lbdResult.Text = result.ToString();
            lbdResult.ForeColor = result ? Color.Blue : Color.Red;
        }

        protected void CreateOrderConfirmationEmailTest(object sender, EventArgs e)
        {
            var order = CheckoutService.GetFullOrder("3E397EA0-CB22-46B4-84A0-7AA31CF0F29E");

            CreateOrderConfirmationEmail(order);
        }
    }
}