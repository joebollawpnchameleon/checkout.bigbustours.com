using bigbus.checkout.data.Model;
using Common.Model;
using Services.Infrastructure;
using System;

namespace Services.Implementation
{
    public class NotificationService : BaseService, INotificationService
    {
        public virtual bool CreateHtmlEmail()
        {
            return true;
        }

        public virtual string CreateOrderConfirmationEmail(OrderConfirmationEmailRequest request)
        {
            var siteTemlate = MicrositeEmailRepository.GetSingle(x =>
                    x.MicrositeId.Equals(request.CityName));

            if (siteTemlate == null)
                return "Microsite Email template not found!";

            var template = EmailTemplateRepository.GetSingle(x => x.Id.Equals(siteTemlate.EmailTemplateId));

            if (template == null)
                return "Email template not found!";

            var email = new Email
            {
                 HTMLBody = FillEmailVariables(template.Body, request),
                 DateCreated = DateTime.Now,
                 //Subject = request.      
            };

            return string.Empty;
        }

        private string FillEmailVariables(string emailContent, OrderConfirmationEmailRequest request)
        {
            return string.Empty;
        }
    }
}
