using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    public interface IEmailService
    {
        void InitSmtp(string smtpServer, string username, string password);

        bool SendEmailHTML(string toAddress, string toName, string fromAddress, string fromName, string subject, string body);

        bool SendEmail(string toAddress, string toName, string fromAddress, string fromName, string subject, string body);

        bool SendMultiPartEmail(string sender, string recipients, string subject, string textBody, string htmlBody);

        bool SendEmailWithAttachment(HtmlEmailStruct htmlEmailStruct);

    }
}
