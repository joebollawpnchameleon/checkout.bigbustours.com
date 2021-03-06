﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.HtmlControls;
using bigbus.checkout.Controls;
using bigbus.checkout.data.Model;
using bigbus.checkout.Helpers;
using bigbus.checkout.Models;
using Common.Enums;
using Common.Model;
using Services.Implementation;
using Services.Infrastructure;
using System.Threading;

namespace bigbus.checkout
{
    public partial class Voucher : BasePage
    {
        private Order _order;
        private int _attractionCount;
        private List<EcrOrderLineData> _orderlineData;
 
        public bool IsTradeTicketSale;
        public List<VoucherTicket> MainList = new List<VoucherTicket>();

        protected void Page_Load(object sender, EventArgs e)
        {
            var orderId = Request.QueryString["oid"];

            if (string.IsNullOrEmpty(orderId)) return;

            _order = CheckoutService.GetFullOrder(orderId);

            if (_order == null) return;

            _orderlineData = CheckoutService.GetOrderLineDetails(orderId);

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

                var selectedOrderLines = _order.OrderLines.Where(a => versionGroup.ToList().Any(x =>
                   x.OrderLineId.Equals(a.Id.ToString(), StringComparison.CurrentCultureIgnoreCase))).ToList();

                if(ecrVersionId == (int)EcrVersion.Three)
                    LoadEcr3Tickets(selectedOrderLines);
                else if (ecrVersionId == (int) EcrVersion.One)
                    LoadEcr1Tickets(selectedOrderLines);


                PopulateVoucherTickets();
            }
            

        }

        private void LoadEcr3Tickets(List<OrderLine> orderLines){

            _attractionCount = orderLines.Count(a => 
                a.TicketTorA.Equals("attraction", StringComparison.CurrentCultureIgnoreCase));
            
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
                //get corresponding orderlines  *** when u save ticket id in barcodes, get our ticketid from ecr productuid
                var tempOrderLines = orderLines.Where(x => x.TicketId != null && x.TicketId.Value.ToString().Equals(barcode.TicketId, StringComparison.CurrentCultureIgnoreCase));
                
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
                    
                var voucherTicket =
                    new VoucherTicket
                    {
                        UseQrCode = true,
                            OrderLines = tempOrderLines.ToList(),
                            Ticket = ticket,
                            ImageData = ImageDbService.GetImageMetaData(barcode.ImageId),
                            ValidTicketName = validTicketName
                    };

                //only load ticket image if we have an attraction
                if (ticket.IsAttraction)
                {
                    voucherTicket.AttractionImageData = ticket.ImageMetaDataId != null
                       ? ImageDbService.GetMetaData(ticket.ImageMetaDataId.Value.ToString())
                       : null;
                }
            }

            //PopulateVoucherTickets();
        }

        private void LoadVoucherTicketWithBarcode(List<OrderLine> orderLines)
        {
            foreach (var orderline in orderLines)
            {
                var tempList = MakeVoucherTicketWithBarcode(orderline);
                if (tempList != null && tempList.Count > 0)
                    MainList.AddRange(tempList);
            }
        }
        
        private List<VoucherTicket> MakeVoucherTicketWithBarcode(OrderLine orderLine)
        {
            if (orderLine.TicketId == null)
                return null;

            var allTickets = new List<VoucherTicket>();
            var ticket = TicketService.GetTicketById(orderLine.TicketId.Value.ToString());
            var microsite = SiteService.GetMicroSiteById(orderLine.MicrositeId);

            var validTicketName = ticket.Name.ToLower().Contains(microsite.Name.ToLower())
                ? ticket.Name
                : string.Concat(microsite.Name, " ", ticket.Name);

            var orderlineGeneratedBarcodes = BarcodeService.GetOrderLineGeneratedBarcodes(orderLine.Id.ToString());

            if (orderlineGeneratedBarcodes == null || !orderlineGeneratedBarcodes.Any())
                return null;

            foreach (var barcode in orderlineGeneratedBarcodes)
            {
                var barcodePath = BarCodeDir + barcode.GeneratedBarcode + ".jpg";
                ImageService.DoesBarCodeImageExist(barcode.GeneratedBarcode, Server.MapPath(barcodePath));
                allTickets.Add(
                    new VoucherTicket
                    {
                        UseQrCode = false,
                        OrderLines = new List<OrderLine>{orderLine},
                        Ticket = ticket,
                        BarCodeFixQuantity = 1,
                        BarCodeImageUrl = barcodePath,
                        ValidTicketName = validTicketName,
                        BarCode = barcode.GeneratedBarcode
                    }
                );
            }

            return allTickets;
        }

       
        private void LoadAttractionsForEcr1(List<OrderLine> attractionOrderLines)
        {
            // group these orderlines by ticketid
            var ticketGroups =
            from detail in attractionOrderLines
            group detail by detail.TicketId into ticketGroup
            orderby ticketGroup.Key
            select ticketGroup;

            //loog through all ticket groups and make vouchers
            foreach(var ticketgroup in ticketGroups)
            {
                var ticketId = ticketgroup.Key.Value.ToString();
                var ticketOrderlines = attractionOrderLines.Where(x => 
                    x.TicketId.Value.ToString().Equals(ticketId, StringComparison.CurrentCultureIgnoreCase)
                    );

                //since they all have the same ticket lest get the first one to work with
                var topOrderLine = ticketOrderlines.FirstOrDefault();
                //get corresponding site data
                var orderLineData = _orderlineData.FirstOrDefault(x =>
                    x.OrderLineId.Equals(topOrderLine.Id.ToString(),
                    StringComparison.CurrentCultureIgnoreCase));
                //only generate qr code if site supports qr code
                if (orderLineData.UseQrCode)
                {
                    //make the qr image
                    var filePath = MakeAttractionQrCode(_order, ticketOrderlines.ToList(), ticketId);
                    //*** use file path and create the Voucher.
                    var ticket = TicketService.GetTicketById(ticketId);

                    var validTicketName = ticket.Name.ToLower().Contains(orderLineData.MicrositeName)
                   ? ticket.Name
                   : string.Concat(orderLineData.MicrositeName, " ", ticket.Name);

                    var attractionMetaData = ticket.ImageMetaDataId != null
                        ? ImageDbService.GetMetaData(ticket.ImageMetaDataId.Value.ToString())
                        : null;

                    MainList.Add(
                            new VoucherTicket
                            {
                                UseQrCode = true,
                                OrderLines = ticketOrderlines.ToList(),
                                Ticket = ticket,
                                AttractionImageData = attractionMetaData,
                                ValidTicketName = validTicketName,
                                QrCodeImageUrl = filePath
                            }
                        );
                }
            }

           
        }

        private void LoadEcr1Tickets(List<OrderLine> orderLines)
        {
            var tourOrderLines = orderLines.Where(x => x.IsTour);
            var attractionOrderLines = orderLines.Where(x => x.IsAttraction);

            if (attractionOrderLines.Any())
            {
                LoadAttractionsForEcr1(attractionOrderLines.ToList());
            }

            if (!tourOrderLines.Any())
                return;

            var ticketGroups =
             from detail in tourOrderLines
             group detail by detail.TicketId into ticketGroup
             orderby ticketGroup.Key
             select ticketGroup;

            //we need to have at least one group to proceed
            if (!ticketGroups.Any())
            {
                Log("Voucher => LoadEcr1Tickets() Ticketgroup empty orderid: " + _order.Id);
                return;
            }
            
            //if we have more than 1 ticket or more than 1 site then print barcodes automatically or site not support qr code
            if (ticketGroups.Count() > 1)
            {
                LoadVoucherTicketWithBarcode(tourOrderLines.ToList());
                return;
            }

            var firstGroup = ticketGroups.FirstOrDefault();

            if (firstGroup == null || firstGroup.Key == null)
            {
                Log("Voucher => LoadEcr1Tickets() No group found. orderid: " + _order.Id);
                return;
            }
            
            var ticketId = firstGroup.Key.Value.ToString();
            var ticket = TicketService.GetTicketById(ticketId);
            var microsite = SiteService.GetMicroSiteById(ticket.MicroSiteId);

            //make sure this one site supports qr otherwise, do barcodes
            if (!microsite.UseQR)
            {
                LoadVoucherTicketWithBarcode(tourOrderLines.ToList());
                return;
            }

            var voucherTicket = LoadVoucherTicketWithQrcode(_order, tourOrderLines.ToList(), ticket, microsite);
            MainList.Add(voucherTicket);
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
                voucherControl.QrCodeSupported = voucherTicket.UseQrCode;

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
