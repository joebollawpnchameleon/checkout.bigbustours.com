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
            LeadName = Order.PaymentMethod.Equals("Paypal", StringComparison.CurrentCultureIgnoreCase)? Order.UserName : Order.NameOnCard;
            OrderNumber = Order.OrderNumber.ToString();
            IsTradeTicketSale = false;
            VoucherPrice = Order.Currency.Symbol + VoucherTicket.OrderLines.Sum(x => x.NettOrderLineValue ?? (decimal)0.0);
            OrderTotal = Order.Currency.Symbol + Order.Total;

           // MakeSureImageExists();
            ImageWidth = VoucherTicket.ImageData.Width;
            ImageHeight = VoucherTicket.ImageData.Height;
            ImageId = VoucherTicket.ImageData.ImageId.ToString();
            ImageExtension = VoucherTicket.ImageData.Type;

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

            var sum = orderLines.Sum(x => x.TicketQuantity);

            return sum ?? 0;
        }

        //private void MakeSureImageExists()
        //{
        //    var imageMetaData = VoucherTicket.ImageData;
        //    var image = imageMetaData.RelatedImage;

        //    ImageWidth = imageMetaData.Width;
        //    ImageHeight = imageMetaData.Height;

        //    ImageId = imageMetaData.ImageId.ToString();


        //    //use image service check that file with this image id exist if it does display image file if not use httphandler to create file and then display it.
        //    var imagePath = Server.MapPath(string.Format("~/FileUploadPath/{0}/{1}{2}", MicrositeId, ImageId, imageMetaData.Type));

        //    var file = new FileInfo(imagePath);

        //    if (!file.Exists)//create it
        //    {
        //        var stream = new System.IO.MemoryStream(image.Data);
        //        var newstream = new MemoryStream();

        //        if (file.DirectoryName != null)
        //        {
        //            var dir = Directory.CreateDirectory(file.DirectoryName);
        //            System.Drawing.Image i = System.Drawing.Image.FromStream(stream);
        //            System.Drawing.Image newi = null;
        //            if (width > 0 && height > 0 && (width != i.Width || height != i.Height))
        //            {
        //                if (keepRatio)
        //                    newi = ImageService.ScaleImageToFixedSize(i, new System.Drawing.Size(width, height));
        //                else
        //                    newi = ImageService.ResizeImage(i, new System.Drawing.Size(width, height));

        //                newi.Save(newstream, i.RawFormat);
        //            }
        //            else if (width > 0 && width != i.Width)
        //            {
        //                newi = ImageService.ScaleImageToWidth(i, width);
        //                newi.Save(newstream, i.RawFormat);
        //            }
        //            else if (height > 0 && height != i.Height)
        //            {
        //                newi = ImageService.ScaleImageToHeight(i, height);
        //                newi.Save(newstream, i.RawFormat);
        //            }
        //            else if (square > 0)
        //            {
        //                if (i.Width >= i.Height)
        //                    newi = ImageService.ScaleImageToHeight(i, square);
        //                else
        //                    newi = ImageService.ScaleImageToWidth(i, square);

        //                int cx = (newi.Width / 2) - (square / 2);
        //                int cy = (newi.Height / 2) - (square / 2);
        //                newi = ImageService.CropImage(newi, cx, cy, square, square);
        //                newi.Save(newstream, i.RawFormat);
        //            }
        //            else
        //            {
        //                newi = i;
        //                newstream = stream;
        //            }

        //            newi.Save(file.FullName, i.RawFormat);
        //            newi.Dispose();
        //            i.Dispose();
        //        }

        //        var fs = file.Create();
        //        // Modify the file as required, and then close the file.
        //        fs.Close();
        //        // Delete the file.
        //        file.Delete();
        //    }
        //    //if it doesn't exist, get image data and create file using ImageDB and passing bytes to ImageService

        //}
       
    }
}