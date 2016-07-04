using bigbus.checkout.data.Model;
using Common.Model;
using Services.Infrastructure;
using System;
using System.Globalization;
using System.Text;

namespace Services.Implementation
{
    public class NotificationService : BaseService, INotificationService
    {
        public virtual bool CreateHtmlEmail()
        {
            return true;
        }

        #region testing only
        public Email GetEmail(string toAddress)
        {
            return
                EmailRepository.GetSingle(
                    x => x.ToAddresses.Equals(toAddress, StringComparison.CurrentCultureIgnoreCase));
        }
        #endregion

        public Email GetEmailById(string emailId)
        {
            return EmailRepository.GetSingle(x => x.Id.ToString().Equals(emailId, StringComparison.CurrentCultureIgnoreCase));
        }

        public Email GetEmailByOrderId(string orderId)
        {
            return EmailRepository.GetSingle(x => x.OrderId.Equals(orderId, StringComparison.CurrentCultureIgnoreCase));
        }

        public virtual EmailTemplate GetEmailTemplate(string templateId)
        {
            return EmailTemplateRepository.GetSingle(x => x.Id.ToString().Equals(templateId, StringComparison.CurrentCultureIgnoreCase));
        }

        public virtual MicrositeEmailTemplate GetSiteEmailTemplate(string micrositeId, string languageId)
        {
            return MicrositeEmailRepository.GetSingle(x =>
                  x.MicrositeId.Equals(micrositeId, StringComparison.CurrentCultureIgnoreCase)
                  && x.LanguageId.Equals(languageId, StringComparison.CurrentCultureIgnoreCase));    
        }

        public virtual string CreateOrderConfirmationEmail(OrderConfirmationEmailRequest request)
        {
            var email = new Email
            {
                 HTMLBody = FillEmailVariables(request),
                 DateCreated = DateTime.Now,
                 Subject = request.EmailSubject,
                 FromAddress = request.SenderEmail,
                 ToAddresses = request.ReceiverEmail,
                 BCCAddresses = request.CcEmails,
                 PriorityLevel = 3,
                 ReadyToSend = true,
                 OrderId = request.OrderId
            };

            EmailRepository.Add(email);

            return string.Empty;
        }
        

        public static string FillEmailVariables(OrderConfirmationEmailRequest request)
        {
            var sbTemp = new StringBuilder(request.HtmlBody);

            var cityName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(request.CityName.ToLower());

            sbTemp
                .Replace("[@App_Store@]", request.AppStoreLink)
                .Replace("[@Google_Play@]", request.GooglePlayLink)
                .Replace("[@View_In_Browser@]", request.ViewInBrowserLink)
                .Replace("[Email_Subject]", request.EmailSubject)
                .Replace("[Customer_First_Name]", request.ReceiverFirstname)
                .Replace("[City_Name]", cityName)
                .Replace("[Order_Number]", request.OrderNumber)
                .Replace("[@View_And_print_ticket@]", request.ViewAndPrintTicketLink)
                .Replace("[Ticket_Details]", request.TicketDetails)
                .Replace("[User_Full_Name]", request.UserFullName)
                .Replace("[Date_Of_Order]", request.DateOfOrder)
                .Replace("[Order_Total]", request.OrderTotal)
                .Replace("[Ticket_Quantity]", request.TicketQuantity)
                .Replace("[@terms_and_conditions@]", request.TermsAndConditionsLink)
                .Replace("[@privacy_policy@]", request.PrivacyPolicyLink)
                .Replace("[@App_Store@]", request.AppStoreLink)
                .Replace("[@Google_Play@]", request.GooglePlayLink)
                .Replace("[City_Number]", request.CityNumber)
                .Replace("[City_Email]", request.CityEmail)
                .Replace("[@Contact_Us@]", request.ContactUsLink)
                .Replace("[@Faqs@]", request.FaqLink)
                .Replace("[@Download_Map@]", request.DownloadMapLink)
                .Replace("[@Trip_Advisor@]", request.TripAdvisorLink)
                .Replace("[@Trust_Pilot@]", request.TrustPilotLink);

            return sbTemp.ToString();
        }

        public virtual ContactData GetSiteContactData(string micrositeId, string page)
        {
            return ContactsRepository.GetSingle(x => x.MicroSiteId.Equals(micrositeId, StringComparison.CurrentCultureIgnoreCase) 
                && x.Page.Equals(page, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
