using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace bigbus.checkout
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        public void AddMetas(string metas)
        {
            ltAdditionalMetas.Text += metas;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}