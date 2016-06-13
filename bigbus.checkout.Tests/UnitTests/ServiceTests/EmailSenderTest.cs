

using Common.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Implementation;
using Services.Infrastructure;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Reflection;

namespace bigbus.checkout.Tests.UnitTests.ServiceTests
{
    [TestClass]
    public class EmailSenderTest
    {
        private SmtpSettings _settings;
        private IEmailService _service;
        private string _mailPickupDir;
        private string LocalPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);                
            }
        }

        public EmailSenderTest()
        {
            _settings = new SmtpSettings
            {
                UserName = ConfigurationManager.AppSettings["SMTPUsername"],
                Password = ConfigurationManager.AppSettings["SMTPPass"],
                SMTPServer = ConfigurationManager.AppSettings["SMTPServerName"]
            };

            _mailPickupDir = ConfigurationManager.AppSettings["MailPickupDir"];
            _service = string.IsNullOrEmpty(_mailPickupDir)? new EmailService(_settings) : new EmailService(_settings, _mailPickupDir);
        }

        [TestMethod]
        public void CanSendHtmlEmail()
        {            
            var htmlFilePath = Path.Combine(LocalPath.Replace(@"\bin\Debug", ""), "TestFiles", "eng-standard.html");
            var body = string.Empty;

            using (var reader = File.OpenText(htmlFilePath)) // Path to your 
            {            
                body  = reader.ReadToEnd();
            }

            var result = _service.SendEmailHTML("joe.bolla@wpnchameleon.co.uk", "Joseph Francis Bolla", "admin@cyberminds.co.uk", "Cyberminds Test", "Test Email", body);

            Assert.IsTrue(result);
        }
    }
}
