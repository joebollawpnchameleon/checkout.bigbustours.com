using System;
using System.Collections.Generic;
using bigbus.checkout.Models;


namespace bigbus.checkout.Controls
{
    public partial class BasketDisplay : System.Web.UI.UserControl
    {
        public List<BasketDisplayVm> DataSource { get; set; }
 
        public BasePage ParentPage { get; set; }

        public string TotalString { get; set; }

        public string AddMoreUrl { get; set; }

        public bool ShowActionRow { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rptBasketLines.DataSource = DataSource;
                rptBasketLines.DataBind();
            }
        }

        protected string DisplayMode
        {
            get { return ShowActionRow ? "display:block" : "display:none"; }
        }

        protected void AddMoreTickets(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(AddMoreUrl))
                Response.Redirect(AddMoreUrl);
        }
    }
}