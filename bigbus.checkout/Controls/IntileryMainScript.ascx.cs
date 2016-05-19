
using System.Configuration;

namespace bigbus.checkout.Controls
{
    public partial class IntileryMainScript : System.Web.UI.UserControl
    {
        protected string AccountId
        {
            get { return ConfigurationManager.AppSettings["IntileryAccountId"]; }
        }
    }
}