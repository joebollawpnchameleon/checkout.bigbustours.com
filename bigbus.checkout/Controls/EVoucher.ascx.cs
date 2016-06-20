using bigbus.checkout.data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bigbus.checkout;
using bigbus.checkout.Models;
using System.IO;
using System.Text;
using Services.Implementation;

namespace bigbus.checkout.Controls
{
    public partial class EVoucher : System.Web.UI.UserControl
    {
        public VoucherTicket VoucherTicket { get; set; }
        public Order Order { get; set; }
        public string ValidTicketName { get; set; }
        public string MicrositeId { get; set; }
        public string OpenDayTranslation { get; set; }
        public bool ShowOrderTotal { get; set; }
        public bool QrCodeSupported { get; set; }

        protected string TicketName { get; set; }
        protected string LeadName { get; set; }
        protected string OrderNumber { get; set; }
        protected bool IsTradeTicketSale { get; set; }
        protected string VoucherPrice { get; set; }
        protected string OrderTotal { get; set; }
        protected string TicketLine1 { get; set; }
        protected string TicketLine2 { get; set; }
        protected string TicketLine3 { get; set; }
        protected int AdultQuantity { get; set; }
        protected int ChildQuantity { get; set; }
        protected int FamilyQuantity { get; set; }
        protected string PaymentType { get; set; }
        protected string CcNumber { get; set; }
        protected int ImageWidth { get; set; }
        protected int ImageHeight { get; set; }
        protected string ImageId { get; set; }
        protected string ImageExtension { get; set; }
        protected string QrImageUrl { get; set; }

        private Ticket _ticket;

        public string CodeImageUrl
        {
            get
            {
                return QrCodeSupported
                    ? ResolveUrl("~/QrCodeImageHandler.ashx")
                    : ResolveUrl("~/BarCodeImageHandler.ashx");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (VoucherTicket != null && Order != null)
            {
                LoadVoucherDetails();
                DisplayCodeImage();
            }
        }        

        private void DisplayCodeImage()
        {
            QrImageUrl = _ticket.IsAttraction?
                VoucherTicket.QrCodeImageUrl : CodeImageUrl + "?w=200&h=200&extension=" + ImageExtension 
                + "&micrositeid=" + MicrositeId + "&imageid=" + ImageId;
            dvQrcode.Visible = VoucherTicket.UseQrCode;
            dvBarcode.Visible = !VoucherTicket.UseQrCode;
        }

        protected void LoadVoucherDetails()
        {
            _ticket = VoucherTicket.Ticket;
            TicketName = _ticket.Name;
            LeadName = Order.PaymentMethod.Equals("Paypal", StringComparison.CurrentCultureIgnoreCase)? Order.UserName : Order.NameOnCard;
            OrderNumber = Order.OrderNumber.ToString();
            IsTradeTicketSale = false;
            VoucherPrice = Order.Currency.Symbol + VoucherTicket.OrderLines.Sum(x => x.NettOrderLineValue ?? (decimal)0.0);
            OrderTotal = Order.Currency.Symbol + Order.Total;

            //*** MakeSureImageExists();
            if (VoucherTicket.UseQrCode && VoucherTicket.ImageData != null)
            {
                ImageWidth = VoucherTicket.ImageData.Width;
                ImageHeight = VoucherTicket.ImageData.Height;
                ImageId = VoucherTicket.ImageData.ImageId.ToString();
                ImageExtension = VoucherTicket.ImageData.Type;
            }
            //else
            //{
            //    ImageId = VoucherTicket.ImageData.ImageId.ToString();
            //    ImageExtension = VoucherTicket.ImageData.Type;
            //}

            TicketLine1 = GetTicketLine1();

            TicketLine2 = string.IsNullOrWhiteSpace(_ticket.TicketTextLine2)
                ? string.Empty
                : _ticket.TicketTextLine2.Trim();

            TicketLine3 = string.IsNullOrWhiteSpace(_ticket.TicketTextLine3)
                ? string.Empty
                : _ticket.TicketTextLine3.Trim();

            AdultQuantity = GetQuantityByUserType("Adult");
            ChildQuantity = GetQuantityByUserType("Child");
            FamilyQuantity = GetQuantityByUserType("Family");
            
            if (_ticket.IsAttraction && VoucherTicket.AttractionImageData != null)
            {
                imgAttraction.Visible = true;
                imgAttraction.Alt = "";
                imgAttraction.Src = ResolveUrl("~/GenericImageHandler.ashx?w=124&h=102&extension=") +
                                    VoucherTicket.AttractionImageData.Type
                                    + "&micrositeid=" + MicrositeId + "&imageid=" +
                                    VoucherTicket.AttractionImageData.ImageId;
            }

           if (Order.PaymentMethod.Equals("paypal", StringComparison.CurrentCultureIgnoreCase))
            {
                PaymentType =  "PayPal";
            }
            else
            {
                PaymentType = "CC Number:";
                CcNumber = !string.IsNullOrEmpty(Order.CcLast4Digits) ? Order.CcLast4Digits : "****";
            }
        }

        private string GetTicketLine1()
        {
            var sbTemp = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(_ticket.TicketTextTopLine))
                sbTemp.Append(_ticket.TicketTextTopLine + " ");

            if (!string.IsNullOrWhiteSpace(_ticket.TicketTextMiddleLine))
                sbTemp.Append(_ticket.TicketTextMiddleLine + " ");

            if (!string.IsNullOrWhiteSpace(_ticket.TicketTextBottomLine))
                sbTemp.Append(_ticket.TicketTextBottomLine + " ");

            return sbTemp.ToString();
        }

        private int GetQuantityByUserType(string userType)
        {
            var userTypeLine =
                VoucherTicket.OrderLines.Where(
                    x => x.TicketType.Equals(userType, StringComparison.CurrentCultureIgnoreCase));

            var orderLines = userTypeLine as OrderLine[] ?? userTypeLine.ToArray();

            if (!orderLines.Any())
                return 0;

            if (!VoucherTicket.UseQrCode)
                return 1;

            var sum = orderLines.Sum(x => x.TicketQuantity);

            return sum ?? 0;
        }

     
    }
}