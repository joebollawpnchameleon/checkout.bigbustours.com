
using System.Configuration;

namespace bigbus.checkout.Models
{
    public class AdminBasePage : BasePage
    {
        protected string AdminUploadPath { get { return ConfigurationManager.AppSettings["AdminUploadPath"]; } }

    }
}