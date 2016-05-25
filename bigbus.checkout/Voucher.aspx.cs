//using System;
//using System.Collections.Generic;
//using System.Linq;
//using bigbus.checkout.Models;
//using bigbus.checkout.data.Model;
//using Services.Infrastructure;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.HtmlControls;
using bigbus.checkout.Controls;
using bigbus.checkout.data.Model;
using bigbus.checkout.Helpers;
using bigbus.checkout.Models;
using Services.Implementation;
using Services.Infrastructure;

namespace bigbus.checkout
{
    public partial class Voucher : BasePage
    {
        private Order _order;
        private new List<OrderLine> _allOrderLines;
      
        private bool _isCashSale;
        private bool _isRemittanceSale;
        private int _attractionCount;

        public bool IsTradeTicketSale;

        public List<VoucherTicket> MainList = new List<VoucherTicket>();               
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //EnsureSessionCorrectlyEstablished(); // uses virtual function so logic can be overridden

            //EstablishURL();
            var orderId = Request.QueryString["oid"];

            if (string.IsNullOrEmpty(orderId)) return;

            _order = CheckoutService.GetFullOrder(orderId);

            if (_order == null) return;

            _allOrderLines = _order.OrderLines.ToList();

            //var isPayPalTransaction = _order.PaymentMethod.Equals("PAYPAL", StringComparison.OrdinalIgnoreCase);
            //var tourOrderLines = _allOrderLines.Where(a => a.TicketTorA == "Tour").ToList();

            _attractionCount = _allOrderLines.Count(a => 
                a.TicketTorA.Equals("attraction", StringComparison.CurrentCultureIgnoreCase));//.ToList();

            
            //get all Ecr barcodes
            var barcodes = ImageDbService.GetOrderEcrBarcodes(_order.OrderNumber);

            //make sure we have some barcodes
            if(barcodes == null || barcodes.Count < 1)
            {
                Log("Barcodes failed to retrieve for ordernumber: " + _order.OrderNumber);
                return;
            }

            //group orderlines by barcode images
            foreach(var barcode in barcodes.ToList())
            {
                var imageId = barcode.ImageId; //retrieve img metadata from this

                //get corresponding orderlines  *** when u save ticket id in barcodes, get our ticketid from ecr productuid
                var tempOrderLines = _allOrderLines.Where(x => x.TicketId != null && x.TicketId.Value.ToString().Equals(barcode.TicketId, StringComparison.CurrentCultureIgnoreCase));
                
                if(!tempOrderLines.Any())
                    continue;
                
                var ticket = TicketService.GetTicketById(barcode.TicketId);
                
                var topOrderLine = tempOrderLines.FirstOrDefault();

                if(topOrderLine == null)
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
                             OrderLines = tempOrderLines.ToList(),
                             Ticket = ticket,
                             AttractionImageData = attractionMetaData,
                             ImageData = ImageDbService.GetImageMetaData(barcode.ImageId),
                             ValidTicketName = validTicketName
                        }
                    );
            }

            PopulateVoucherTickets();
        }

        protected void PopulateVoucherTickets()
        {
            var counter = 0;
            var lineisbroke = false;

            foreach (var voucherTicket in MainList)
            {
                counter++;

                if (counter > 1 && voucherTicket.IsAttraction && !lineisbroke)
                {
                    AddPageBreak();
                }

                //add voucher control
                var voucherControl = (EVoucher) LoadControl("~/Controls/EVoucher.ascx");
                voucherControl.Order = _order;
                voucherControl.VoucherTicket = voucherTicket;
                voucherControl.ValidTicketName = voucherTicket.ValidTicketName;
                voucherControl.MicrositeId = MicrositeId;
                voucherControl.ShowOrderTotal = _attractionCount > 0;
                voucherControl.OpenDayTranslation = GetTranslation("OpenDayTicket");

                plcAllVouchersContent.Controls.Add(voucherControl);

                if ((voucherTicket.IsAttraction || counter % 3 == 0) && MainList.Count() > counter)
                {
                    lineisbroke = true;
                    AddPageBreak();
                }
                else
                {
                    lineisbroke = false;
                }
            }
    
        }

        private void AddPageBreak()
        {
            var dvPageBreak = new HtmlGenericControl
            {
                InnerHtml = "&nbsp;"
            };

            dvPageBreak.Attributes.Add("class", "page-breaker");
            plcAllVouchersContent.Controls.Add(dvPageBreak);
        }

        private static string FormatDate(DateTime dt)
        {
            var ret = string.Empty;

            switch (dt.Day)
            {
                case 1:
                case 21:
                case 31:
                    ret += dt.Day + "st ";
                    break;

                case 2:
                case 22:
                    ret += dt.Day + "nd ";
                    break;

                case 3:
                case 23:
                    ret += dt.Day + "rd ";
                    break;

                default:
                    ret += dt.Day + "th ";
                    break;
            }

            ret += char.ToUpper(dt.ToString("MMM yyyy")[0]) + dt.ToString("MMM yyyy").Substring(1);

            return ret;
        }

      

       
    }

}
