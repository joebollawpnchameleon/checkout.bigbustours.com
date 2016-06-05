using bigbus.checkout.data.Model;
using System;


namespace bigbus.checkout
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        public void AddMetas(string metas)
        {
            ltAdditionalMetas.Text += metas;
        }

        public Language CurrentLanguage { get; set; }

        public bool IsMobileSession { get; set; }

        public string MicrositeId { get; set; }

        public string HomeUrl { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}