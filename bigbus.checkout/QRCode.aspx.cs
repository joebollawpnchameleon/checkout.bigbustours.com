using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bigbus.checkout.data.Model;
using bigbus.checkout.Helpers;
using bigbus.checkout.Models;

namespace bigbus.checkout
{
    public partial class QrCodeTestWebform : BasePage
    {
        private void GenerateQrCodeForTimedAttractions(Order order, string orderExternalOrderId)
        {
            //try
            //{
                //var externalBarcodes = EVoucherCreationHelper.GetOrderExternalBarcodes(this, orderExternalOrderId);

                //if (externalBarcodes == null || externalBarcodes.Count < 1)
                //    return;

                //var helper = new EVoucherCreationHelper(this, order);
                //var firstCode = externalBarcodes.FirstOrDefault();

                //dvTimeAttractions.Controls.Add(new Label { Text = firstCode.EventDate.Trim().Split()[0] + " at " + firstCode.EventTime + "<br/>" });

                //foreach (var externalBarcode in externalBarcodes)
                //{
                //    var qrCodeFile = helper.MakeTimedAttractionQrCode(order.OrderNumber.ToString(), order.ExternalOrderId,
                //        externalBarcode.Barcode);

                //    var label = new Label
                //    {
                //        Text =
                //            string.Format("<br/>1 {0} <br/>",
                //                externalBarcode.CustomerTypeName)
                //    };

                //    var qrImage = new System.Web.UI.WebControls.Image
                //    {
                //        AlternateText = externalBarcode.Barcode,
                //        ImageUrl = qrCodeFile
                //    };

                //    qrImage.Style.Add("width", "80%");
                    //dvTimeAttractions.Controls.Add(label);
                    //dvTimeAttractions.Controls.Add(qrImage);

            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log("Failed to generate qr codes on mobile ticket for order: " + orderExternalOrderId);
            //}
        }

        //public static List<ExternalOrderBarcode> GetOrderExternalBarcodes(BasePage page, string orderExternalOrderId)
        //{
        //    try
        //    {
        //        return
        //            page.GetObjectFactory()
        //                .GetListOf<ExternalOrder_Barcodes>("*ExternalOrder_Barcode(ExternalOrderId = $p0$)",
        //                    orderExternalOrderId)
        //                .ToList();
        //    }
        //    catch
        //    {
        //        page.Log("External Barcode retrieval failed. for orderid " + orderExternalOrderId);
        //    }

        //    return null;
        //}

        protected void Page_Load(object sender, EventArgs eventArgs)
        {
            try
            {
                var orderId = Request.QueryString["oid"];

                if (string.IsNullOrWhiteSpace(orderId)) return;

                var order = CheckoutService.GetFullOrder(orderId);

                if (order == null) return;

                //***Check all ecr barcodes API£ and barcodes for API1

                //var md =
                //    GetObjectFactory().GetObjectByOQL("*(1)ImageMetaData(Name=$p0$)",
                //        QrCodePrefix + theOrder.OrderNumber) as
                //        IImageMetaData;

                var md = new ImageMetaData();
                if (md == null) return;

                QrImage.ImageUrl = "/"; //+ Chameleon2006.Web.Handlers.ImageHandler.GetImagePathForHandler(md);
                QrImage.AlternateText = md.AltText;
                QrImage.Style.Add("width", "80%");
                   
                var ols = order.OrderLines;
                ltDetails.Text = "<div><p>Big Bus Tours Booking:<br/><br/>";
                ltDetails.Text +=
                    (string.IsNullOrWhiteSpace(order.User.Title) ? "" : (order.User.Title + " ")) +
                    order.User.Firstname + " " +
                    (string.IsNullOrWhiteSpace(order.User.Lastname) ? "" : (order.User.Lastname + " ")) +
                    "<br/><br/>";

                var validDate = DateUtil.NullDate;
                var olist =
                    new List<OrderLine>(ols).Where(
                        a =>
                            a.IsTour && a.TicketDate != DateUtil.NullDate &&
                            a.TicketDate != new DateTime()).ToList();

                if (olist.Any())
                {
                    var tempOrder = olist.OrderBy(a => a.TicketDate).FirstOrDefault();
                    validDate = (tempOrder != null && tempOrder.TicketDate != null) ? tempOrder.TicketDate.Value : validDate;
                }



                ltDetails.Text += (validDate == DateUtil.NullDate ? "Open date ticket" : validDate.ToString("dd MMM yyyy")) +
                                  "<br/><br/>";

                foreach (var orderLine in ols)
                {
                    ltDetails.Text += orderLine.TicketQuantity + "&nbsp;" + orderLine.Ticket.Name + 
                        " (" + orderLine.MicroSite.Name + " " + orderLine.TicketTorA + ") " + orderLine.TicketType + "<br/><br/>";
                }

                ltDetails.Text += "Credit card: ***" + (!string.IsNullOrWhiteSpace(order.CcLast4Digits) ? order.CcLast4Digits : "N/A") + "</p></div><br/>";

                if (string.IsNullOrEmpty(order.ExternalOrderId)) return;

                //GenerateQrCodeForTimedAttractions(order, order.ExternalOrderId);
                
            }
            catch (Exception e1)
            {
                var te = e1.Message;
            }
        }
    }
}