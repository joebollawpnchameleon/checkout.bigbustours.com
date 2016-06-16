using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bigbus.checkout.data.Model;
using bigbus.checkout.Helpers;
using bigbus.checkout.Models;
using Common.Enums;
using System.Text;

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
        List<ProductStruct> AllTickets = new List<ProductStruct>();

        protected void Page_Load(object sender, EventArgs eventArgs)
        {
            try
            {
                var orderId = Request.QueryString["oid"];

                if (string.IsNullOrWhiteSpace(orderId)) return;

                var order = CheckoutService.GetFullOrder(orderId);

                if (order == null) return;

                var _orderlineData = CheckoutService.GetOrderLineDetails(orderId);

                var queryVersionGroups =
                  from detail in _orderlineData
                  group detail by detail.NewCheckoutVersionId into versionGroup
                  orderby versionGroup.Key
                  select versionGroup;

                if (!queryVersionGroups.Any())
                {
                    return;
                }

                foreach (var versionGroup in queryVersionGroups)
                {
                    var ecrVersionId = versionGroup.Key;

                    var selectedOrderLines = order.OrderLines.Where(a => versionGroup.ToList().Any(x =>
                       x.OrderLineId.Equals(a.Id.ToString(), StringComparison.CurrentCultureIgnoreCase))).ToList();

                    if (ecrVersionId == (int)EcrVersion.Three)
                        LoadEcr3Tickets(selectedOrderLines);
                    //else if (ecrVersionId == (int)EcrVersion.One)
                    //    LoadEcr1Tickets(selectedOrderLines);


                    //PopulateVoucherTickets();  
                }              
            }
            catch (Exception e1)
            {
                var te = e1.Message;
            }
        }
            

       private void LoadTicket(Order order, OrderLine selectedLine) {

            var sbDetails = new StringBuilder();
            var imageHtml = @"<img style=""width:80%"" src=""{0}"" alt=""{1}"" />";

            //  var ols = order.OrderLines;
            sbDetails.AppendLine("<div><p>Big Bus Tours Booking:<br/><br/>");
            sbDetails.AppendLine(string.IsNullOrWhiteSpace(order.User.Title) ? "" : (order.User.Title + " "));
            sbDetails.AppendLine(order.User.Firstname + " " +
                    (string.IsNullOrWhiteSpace(order.User.Lastname) ? "" : (order.User.Lastname + " ")) +
                    "<br/><br/>");
            sbDetails.AppendLine(
                selectedLine.TicketDate == null || selectedLine.TicketDate == DateUtil.NullDate ?
                "Open date ticket" : selectedLine.TicketDate.Value.ToString("dd MMM yyyy") +
                                  "<br/><br/>");

            sbDetails.AppendLine(selectedLine.TicketQuantity + "&nbsp;" + selectedLine.Ticket.Name + 
                        " (" + selectedLine.MicroSite.Name + " " + selectedLine.TicketTorA + ") " + selectedLine.TicketType + "<br/><br/>");

            sbDetails.AppendLine("Credit card: ***" + (!string.IsNullOrWhiteSpace(order.CcLast4Digits) ? order.CcLast4Digits : "N/A") + "</p></div><br/>");

            AllTickets.Add(
                new ProductStruct {
                    Details = sbDetails.ToString(),
                    ImageHtml = string.Format(imageHtml, ) });
        }

        private void LoadEcr3Tickets(List<OrderLine> orderLines)
        {

            _attractionCount = orderLines.Count(a =>
                a.TicketTorA.Equals("attraction", StringComparison.CurrentCultureIgnoreCase));

            //get all Ecr barcodes
            var barcodes = ImageDbService.GetOrderEcrBarcodes(_order.OrderNumber);

            //make sure we have some barcodes
            if (barcodes == null || barcodes.Count < 1)
            {
                Log("Barcodes failed to retrieve for ordernumber: " + _order.OrderNumber);
                return;
            }

            //group orderlines by barcode images
            foreach (var barcode in barcodes.ToList())
            {
                //get corresponding orderlines  *** when u save ticket id in barcodes, get our ticketid from ecr productuid
                var tempOrderLines = orderLines.Where(x => x.TicketId != null && x.TicketId.Value.ToString().Equals(barcode.TicketId, StringComparison.CurrentCultureIgnoreCase));

                if (!tempOrderLines.Any())
                    continue;

                var ticket = TicketService.GetTicketById(barcode.TicketId);

                var topOrderLine = tempOrderLines.FirstOrDefault();

                if (topOrderLine == null)
                    continue;

                var microsite = SiteService.GetMicroSiteById(topOrderLine.MicrositeId);

                var validTicketName = ticket.Name.ToLower().Contains(microsite.Name.ToLower())
                    ? ticket.Name
                    : string.Concat(microsite.Name, " ", ticket.Name);

                var attractionMetaData = ticket.ImageMetaDataId != null
                    ? ImageDbService.GetMetaData(ticket.ImageMetaDataId.Value.ToString())
                    : null;

                MainList.Add(
                        new VoucherTicket
                        {
                            UseQrCode = true,
                            OrderLines = tempOrderLines.ToList(),
                            Ticket = ticket,
                            AttractionImageData = attractionMetaData,
                            ImageData = ImageDbService.GetImageMetaData(barcode.ImageId),
                            ValidTicketName = validTicketName
                        }
                    );
            }

            //PopulateVoucherTickets();
        }

    }

    public class ProductStruct
    {
        public string Details { get; set;}

        public string ImageHtml { get; set; }
    }
}