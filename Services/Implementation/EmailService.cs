using System;
using System.Net;
using System.Net.Mime;
using System.IO;
using System.Net.Mail;
using Services.Infrastructure;
using Common.Model;

namespace Services.Implementation
{
    public class EmailService : IEmailService
    {
        public string signature = "";
        public string html_signature = "";
        NetworkCredential SMTPUserInfo = null;
        SmtpClient emailClient = null;
        bool bSMTPInitialized = false;
                
        public EmailService(SmtpSettings settings)
        {
            SMTPUserInfo = new NetworkCredential(settings.UserName, settings.Password);
            emailClient = new SmtpClient(settings.SMTPServer);
            emailClient.UseDefaultCredentials = false;
            emailClient.Credentials = this.SMTPUserInfo;
            bSMTPInitialized = true;
        }

        public EmailService(SmtpSettings settings, string pickupDirectiroy="")
        {
            SMTPUserInfo = new NetworkCredential(settings.UserName, settings.Password);
            if (string.IsNullOrEmpty(pickupDirectiroy))
            {
                emailClient = new SmtpClient(settings.SMTPServer);
                emailClient.UseDefaultCredentials = false;
                emailClient.Credentials = this.SMTPUserInfo;
            }
            else
            {
                emailClient = new SmtpClient();
                emailClient.PickupDirectoryLocation = pickupDirectiroy;
                emailClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            }

            bSMTPInitialized = true;

        }

        public void InitSmtp(string smtpServer, string username, string password)
        {
            SMTPUserInfo = new NetworkCredential(username, password);
            emailClient = new SmtpClient(smtpServer);
            emailClient.UseDefaultCredentials = false;
            emailClient.Credentials = this.SMTPUserInfo;
            bSMTPInitialized = !string.IsNullOrEmpty(smtpServer) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
        }


        public bool SendEmailHTML(string toAddress, string toName, string fromAddress, string fromName, string subject, string body)
        {
            try
            {
                if (!bSMTPInitialized) throw new Exception("Error: SMTP Not initialized");
                MailMessage message = new MailMessage(fromAddress, toAddress, subject, body/*.Replace(Environment.NewLine, "<br/>")*/);
                message.IsBodyHtml = true;
                emailClient.UseDefaultCredentials = false;
                emailClient.Credentials = SMTPUserInfo;
                emailClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                //Log("EmailService (SendEmailHTML)- email send failed: " + ex.Message);
                return false;
            }
        }

        public bool SendEmail(string toAddress, string toName, string fromAddress, string fromName, string subject, string body)
        {
            try
            {
                if (!bSMTPInitialized) throw new Exception("Error: SMTP Not initialized");
                MailMessage message = new MailMessage(fromAddress, toAddress, subject, body);
                emailClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                //Log("EmailService (SendEmail)- email send failed: " + ex.Message);
                return false;
            }
        }

        public bool SendMultiPartEmail(String sender, String recipients, String subject, String textBody, String htmlBody)
        {
            try
            {
                if (!bSMTPInitialized) throw new Exception("Error: SMTP Not initialized");
                // Initialize the message with the plain text body:
                MailMessage msg = new MailMessage(sender, recipients, subject, textBody);
                // Convert the html body to a memory stream:
                Byte[] bytes = System.Text.Encoding.UTF8.GetBytes(htmlBody);
                MemoryStream htmlStream = new MemoryStream(bytes);
                // Add the HTML body to the message:
                AlternateView htmlView = new AlternateView(htmlStream, MediaTypeNames.Text.Html);
                msg.AlternateViews.Add(htmlView);
                // Ship it!
                emailClient.Send(msg);
                htmlView.Dispose();
                htmlStream.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                //Log("EmailService (SendMultiPartEmail)- email send failed: " + ex.Message);
                return false;
            }
        }

        public bool SendEmailHTML(EmailStruct estruct)
        {
            try
            {
                if (!bSMTPInitialized) throw new Exception("Error: SMTP Not initialized");
                MailMessage message = new MailMessage(estruct.SenderEmail, estruct.ReceiverEmail, estruct.EmailTitle, estruct.EmailBody.Replace(Environment.NewLine, "<br/>"));
                message.IsBodyHtml = true;
                emailClient.UseDefaultCredentials = false;
                emailClient.Credentials = SMTPUserInfo;
                emailClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
               //Log("EmailService (SendEmailHTML) - email send failed: " + ex.Message);
                return false;
            }
        }

        public bool SendEmail(EmailStruct estruct)
        {
            try
            {
                if (!bSMTPInitialized) throw new Exception("Error: SMTP Not initialized");
                MailMessage message = new MailMessage(estruct.SenderEmail, estruct.ReceiverEmail, estruct.EmailTitle, estruct.EmailBody);
                emailClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                //Log("EmailService (SendEmail)- email send failed: " + ex.Message);
                return false;
            }
        }

        public bool SendMultiPartEmail(HtmlEmailStruct htmlEmailStruct)
        {
            try
            {
                if (!bSMTPInitialized) throw new Exception("Error: SMTP Not initialized");
                // Initialize the message with the plain text body:
                MailMessage msg = new MailMessage(htmlEmailStruct.SenderEmail, htmlEmailStruct.ReceiverEmail, htmlEmailStruct.EmailTitle, htmlEmailStruct.EmailBody);
                // Convert the html body to a memory stream:
                Byte[] bytes = System.Text.Encoding.UTF8.GetBytes(htmlEmailStruct.HTMLBody);
                MemoryStream htmlStream = new MemoryStream(bytes);
                // Add the HTML body to the message:
                AlternateView htmlView = new AlternateView(htmlStream, MediaTypeNames.Text.Html);
                msg.AlternateViews.Add(htmlView);
                // Ship it!
                emailClient.Send(msg);
                htmlView.Dispose();
                htmlStream.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                //Log("EmailService (SendMultiPartEmail) - email send failed: " + ex.Message);
                return false;
            }
        }

        public bool SendEmailWithAttachment(HtmlEmailStruct htmlEmailStruct)
        {
            try
            {
                if (!bSMTPInitialized) throw new Exception("Error: SMTP Not initialized");
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(htmlEmailStruct.SenderEmail);
                mail.To.Add(htmlEmailStruct.ReceiverEmail);
                mail.Subject = htmlEmailStruct.EmailTitle;
                mail.Body = htmlEmailStruct.EmailBody;

                if (!string.IsNullOrEmpty(htmlEmailStruct.AttachmentPath))
                {
                    var attachment = new System.Net.Mail.Attachment(htmlEmailStruct.AttachmentPath);

                    mail.Attachments.Add(attachment);
                }
                emailClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                //Log("EmailService (Send Email with attachment) - email send failed: " + ex.Message);
                return false;
            }

        }
    }
}
