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
        private Order _order;

        private readonly List<ProductStruct> _allTickets = new List<ProductStruct>();

        public string IntroDetails { get; set; }
        public string BottomDetails { get; set; }

        protected void Page_Load(object sender, EventArgs eventArgs)
        {
            try
            {
                var orderId = Request.QueryString["oid"];

                if (string.IsNullOrWhiteSpace(orderId)) return;

                _order = CheckoutService.GetFullOrder(orderId);

                if (_order == null) return;

                var orderlineData = CheckoutService.GetOrderLineDetails(orderId);

                var queryVersionGroups =
                  from detail in orderlineData
                  group detail by detail.NewCheckoutVersionId into versionGroup
                  orderby versionGroup.Key
                  select versionGroup;

                if (!queryVersionGroups.Any())
                {
                    return;
                }

                IntroDetails = "<p>Big Bus Tours Booking:</p><p>" +
                    (string.IsNullOrWhiteSpace(_order.User.Title) ? "" : (_order.User.Title + " ")) +_order.User.Firstname + " " +
                        (string.IsNullOrWhiteSpace(_order.User.Lastname) ? "" : (_order.User.Lastname + " ")) +
                        "</p>";

                foreach (var versionGroup in queryVersionGroups)
                {
                    var ecrVersionId = versionGroup.Key;

                    var selectedOrderLines = _order.OrderLines.Where(a => versionGroup.ToList().Any(x =>
                       x.OrderLineId.Equals(a.Id.ToString(), StringComparison.CurrentCultureIgnoreCase))).ToList();

                    if (ecrVersionId == (int)EcrVersion.Three)
                        LoadEcr3Tickets(selectedOrderLines);
                    else if (ecrVersionId == (int)EcrVersion.One)
                        LoadEcr1Tickets(selectedOrderLines);


                    //PopulateVoucherTickets();  
                }

                BottomDetails = string.Format("<p>Order number:{0}</p>", _order.OrderNumber);

                if (_order.PaymentMethod.Equals("paypal", StringComparison.CurrentCultureIgnoreCase))
                    BottomDetails += "<p>Paypal</p>";
                else
                    BottomDetails += ("<p>Credit card: ***" + (!string.IsNullOrWhiteSpace(_order.CcLast4Digits) ? _order.CcLast4Digits : "N/A") + "</p>");
               

                rpProducts.DataSource = _allTickets;
                rpProducts.DataBind();
            }
            catch (Exception e1)
            {
                var te = e1.Message;
            }
        }


        private void LoadEcr1Tickets(List<OrderLine> orderLines)
        {
            var ticketGroups =
             from detail in orderLines
             group detail by detail.TicketId into ticketGroup
             orderby ticketGroup.Key
             select ticketGroup;


            var sbDetails = new StringBuilder();
            var imageHtml = @"<img class=""qr-code"" src=""{0}"" alt=""{1}"" />";

            foreach (var ticketgroup in ticketGroups)
            {
                var ticketId = ticketgroup.Key.Value.ToString();
                var ticketOrderlines = orderLines.Where(x =>
                    x.TicketId.Value.ToString().Equals(ticketId, StringComparison.CurrentCultureIgnoreCase)
                    );

                sbDetails.AppendLine("<p>Date valid: Open date ticket</p>");
                var ticket = TicketService.GetTicketById(ticketId);

                foreach (var line in ticketOrderlines)
                {
                    sbDetails.AppendLine(string.Format("<p>{0}&nbsp;{1} - ({2} {3}) {4}</p>",
                        line.TicketQuantity, ticket.Name, line.MicrositeId, line.TicketTorA, line.TicketType));
                }

                var imageUrl = ticket.IsAttraction ? MakeAttractionQrCode(_order, ticketOrderlines.ToList(), ticketId) : MakeImageQrUrl();
                
                   
                _allTickets.Add(
                       new ProductStruct
                       {
                           Details = sbDetails.ToString(),
                           ImageHtml = string.Format(imageHtml, imageUrl, "QR-Code")
                       }); //Add q
                sbDetails.Clear();
            }
             
        }

        private string MakeImageQrUrl()
        {
            var meta = GetOrderImageMetaData(_order);
            return "/QrCodeImageHandler.ashx?w=200&h=200&extension=" + meta.Type + "&micrositeid=" +
                       MicrositeId + "&imageid=" + meta.ImageId; 
        }

       private void LoadEcr3Ticket(List<OrderLine> selectedLines, Ticket ticket, MicroSite site, ImageMetaData imageData) {

            var sbDetails = new StringBuilder();
            var imageHtml = @"<img class=""qr-code"" src=""{0}"" alt=""{1}"" />";
           
           foreach (var line in selectedLines)
           {
               sbDetails.AppendLine("<p>Open date ticket</p>");
               sbDetails.AppendLine(string.Format("<p>{0}&nbsp;{1} ({2}{3}) {4}</p>",
                   line.TicketQuantity, ticket.Name, site.Name, line.TicketTorA, line.TicketType));
           }

           sbDetails.AppendLine(string.Format("<p>Order number:{0}</p>", _order.OrderNumber));
           sbDetails.AppendLine("<p>Credit card: ***" + (!string.IsNullOrWhiteSpace(_order.CcLast4Digits) ? _order.CcLast4Digits : "N/A") + "</p>");

           //*** get related ecr3 image here.

           var imageUrl = "/QrCodeImageHandler.ashx?w=200&h=200&extension=" + imageData.Type + "&micrositeid=" +
                          MicrositeId + "&imageid=" + imageData.ImageId;

            _allTickets.Add(
                new ProductStruct {
                    Details = sbDetails.ToString(),
                    ImageHtml = string.Format(imageHtml, imageUrl, "QR-Code")
                }); //Add qr image here
        }

        private void LoadEcr3Tickets(List<OrderLine> orderLines)
        {
          
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

                var imageData = ImageDbService.GetImageMetaData(barcode.ImageId);

                LoadEcr3Ticket(tempOrderLines.ToList(), ticket, microsite, imageData);

                //var attractionMetaData = ticket.ImageMetaDataId != null
                //    ? ImageDbService.GetMetaData(ticket.ImageMetaDataId.Value.ToString())
                //    : null;

                //    MainList.Add(
                //            new VoucherTicket
                //            {
                //                UseQrCode = true,
                //                OrderLines = tempOrderLines.ToList(),
                //                Ticket = ticket,
                //                AttractionImageData = attractionMetaData,
                //                ImageData = ImageDbService.GetImageMetaData(barcode.ImageId),
                //                ValidTicketName = validTicketName
                //            }
                //        );
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