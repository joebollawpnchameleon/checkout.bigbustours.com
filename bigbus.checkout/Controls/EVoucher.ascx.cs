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

namespace bigbus.checkout.Controls
{
    public partial class EVoucher : System.Web.UI.UserControl
    {
        public VoucherTicket VoucherTicket { get; set; }
        public Order Order { get; set; }
        public string ValidTicketName { get; set; }
        public string MicrositeId { get; set; }

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

        private Ticket _ticket;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (VoucherTicket != null && Order != null)
                LoadVoucherDetails();
        }

        protected void LoadVoucherDetails()
        {
            _ticket = VoucherTicket.Ticket;
            TicketName = _ticket.Name;
            LeadName = Order.NameOnCard;
            OrderNumber = Order.OrderNumber.ToString();
            IsTradeTicketSale = false;
            VoucherPrice = Order.Currency.Symbol + VoucherTicket.OrderLines.Sum(x => x.GrossOrderLineValue);
            OrderTotal = Order.Currency.Symbol + Order.Total;

            MakeSureImageExists();

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
            
            if (!string.IsNullOrEmpty(VoucherTicket.AttractionImageUrl))
            {
                imgAttractionImage.AlternateText = TicketName;
                imgAttractionImage.ImageUrl = VoucherTicket.AttractionImageUrl;
            }

            //imgQR.AlternateText = "QR-Image";
            //imgQR.ImageUrl = GetImageUrl();

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
            var line1 =

                (string.IsNullOrWhiteSpace(_ticket.TicketTextTopLine) ? string.Empty : _ticket.TicketTextTopLine.Trim() + " ") +
                (string.IsNullOrWhiteSpace(_ticket.TicketTextMiddleLine) ? string.Empty : _ticket.TicketTextMiddleLine.Trim() + " ") +
                (string.IsNullOrWhiteSpace(_ticket.TicketTextBottomLine) ? string.Empty : _ticket.TicketTextBottomLine.Trim() + " ")
            ;

            return !string.IsNullOrEmpty(line1) ? line1.Trim() : "";
        }

        private int GetQuantityByUserType(string userType)
        {
            var userTypeLine =
                VoucherTicket.OrderLines.Where(
                    x => x.TicketType.Equals(userType, StringComparison.CurrentCultureIgnoreCase));

            var orderLines = userTypeLine as OrderLine[] ?? userTypeLine.ToArray();

            if (!orderLines.Any())
                return 0;

            var sum = orderLines.Sum(x => x.TicketQuantity);

            return sum ?? 0;
        }

        private void MakeSureImageExists()
        {
            var imageMetaData = VoucherTicket.ImageData;

            ImageWidth = imageMetaData.Width;
            ImageHeight = imageMetaData.Height;

            ImageId = imageMetaData.ImageId.ToString();

            //use image service check that file with this image id exist if it does display image file if not use httphandler to create file and then display it.
            var imagePath = Server.MapPath(string.Format("~/FileUploadPath/{0}/{1}{2}", MicrositeId, ImageId, imageMetaData.Type));
            var file = new FileInfo(imagePath);

            if (!file.Exists)//create it
            {
                FileStream fs = file.Create();
                // Modify the file as required, and then close the file.
                fs.Close();
                // Delete the file.
                file.Delete();
            }
            //if it doesn't exist, get image data and create file using ImageDB and passing bytes to ImageService

        }
       
    }
}