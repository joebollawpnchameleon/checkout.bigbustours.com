using bigbus.checkout.data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static bigbus.checkout.Voucher;

namespace bigbus.checkout.Controls
{
    public partial class EVoucher : System.Web.UI.UserControl
    {
        public VoucherTicket VoucherTicket { get; set; }
        public Order Order { get; set; }

        protected string TicketName { get; set; }
        protected string LeadName { get; set; }
        protected string OrderNumber { get; set; }
        protected bool IsTradeTicketSale { get; set; }
        protected string VoucherPrice { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (VoucherTicket != null && Order != null)
                LoadVoucherDetails();
        }

        protected void LoadVoucherDetails()
        {
            TicketName = VoucherTicket.Ticket.Name;
            LeadName = Order.NameOnCard;
            OrderNumber = Order.OrderNumber.ToString();
            IsTradeTicketSale = false;
            VoucherPrice = Order.Currency.Symbol + VoucherTicket.OrderLines.Sum(x => x.GrossOrderLineValue).ToString();

            if (!string.IsNullOrEmpty(VoucherTicket.AttractionImageUrl))
            {
                imgAttractionImage.AlternateText = TicketName;
                imgAttractionImage.ImageUrl = VoucherTicket.AttractionImageUrl;
            }             
        }
    }
}