using bigbus.checkout.data.Model;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    public interface INotificationService
    {

        #region testing only

        Email GetEmail(string toAddress);

        #endregion

        bool CreateHtmlEmail();

        string CreateOrderConfirmationEmail(OrderConfirmationEmailRequest request);

        ContactData GetSiteContactData(string micrositeId, string page);

        EmailTemplate GetEmailTemplate(string templateId);

        MicrositeEmailTemplate GetSiteEmailTemplate(string micrositeId, string languageId);
    }


}
