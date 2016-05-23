﻿using bigbus.checkout.data.Model;
using Common.Model;
using Services.Infrastructure;
using System;
using System.Drawing.Text;
using System.Text;

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
                    x.MicrositeId.Equals(request.CityName, StringComparison.CurrentCultureIgnoreCase)
                    && x.MicrositeId.Equals(request.LanguageId, StringComparison.CurrentCultureIgnoreCase));

            if (siteTemlate == null)
                return "Microsite Email template not found!";

            var template = EmailTemplateRepository.GetSingle(x => x.Id.Equals(siteTemlate.EmailTemplateId));

            if (template == null)
                return "Email template not found!";

            var email = new Email
            {
                 HTMLBody = FillEmailVariables(template.Body, request),
                 DateCreated = DateTime.Now,
                 Subject = request.EmailSubject,
                 FromAddress = request.SenderEmail,
                 ToAddresses = request.ReceiverEmail,
                 PriorityLevel = 3,
                 ReadyToSend = true
            };

            return string.Empty;
        }
        

        private string FillEmailVariables(string emailContent, OrderConfirmationEmailRequest request)
        {
            var sbTemp = new StringBuilder(emailContent);

            sbTemp.Replace("[City_Name]", request.CityName)
                .Replace("[Order_Number]", request.OrderNumber)
                .Replace("[@View_And_print_ticket@]", request.ViewAndPrintTicketLink)
                .Replace("[User_Full_Name]", request.UserFullName)
                .Replace("[Date_Of_Order]", request.DateOfOrder)
                .Replace("[Order_Total]", request.OrderTotal)
                .Replace("[Ticket_Quantity]", request.TicketQuantity)
                .Replace("[@terms_and_conditions@]", request.TermsAndConditionsLink)
                .Replace("[@privacy_policy@]", request.PrivacyPolicyLink)
                .Replace("[@App_Store@]", request.AppStoreLink)
                .Replace("[@Google_Play@]", request.GooglePlayLink)
                .Replace("[City_Number]", request.CityNumber)
                .Replace("[City_Email] ", request.CityEmail);

            return sbTemp.ToString();
        }

        public virtual ContactData GetSiteContactData(string micrositeId, string page)
        {
            return ContactsRepository.GetSingle(x => x.MicroSiteId.Equals(micrositeId, StringComparison.CurrentCultureIgnoreCase) 
                && x.Page.Equals(page, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
