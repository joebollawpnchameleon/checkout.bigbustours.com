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
using bigbus.checkout.data.Model;
using bigbus.checkout.Helpers;
using bigbus.checkout.Models;
using Services.Implementation;
using Services.Infrastructure;

namespace bigbus.checkout
{
    public partial class Voucher : BasePage
    {
        private Order _thisOrder;
        private new List<OrderLine> _allOrderLines;
        //private Session _thisSession;

        private bool _isCashSale;
        private bool _isRemittanceSale;

        public bool IsTradeTicketSale;

        public List<VoucherTicket> MainList = new List<VoucherTicket>();

        public ICheckoutService CheckoutService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //EnsureSessionCorrectlyEstablished(); // uses virtual function so logic can be overridden

            //EstablishURL();

            if (string.IsNullOrEmpty(Request.QueryString["Id"])) return;

            var thisOrderId = Request.QueryString["Id"];

            _thisOrder = CheckoutService.GetFullOrder(thisOrderId);

            if (_thisOrder == null) return;

            _allOrderLines = _thisOrder.OrderLines.ToList();

            var isPayPalTransaction = _thisOrder.PaymentMethod.Equals("PAYPAL", StringComparison.OrdinalIgnoreCase);

            //if (!string.IsNullOrWhiteSpace(Request["sid"]))
            //{
            //    _thisSession = GetObjectFactory().GetByOQL<Session>("*(1)Session(Id = $p0$)", Request["sid"]);
            //}

            //var thisAgentOrder = GetObjectFactory().GetByOQL<AgentOrder>("?(*)AgentOrder(Order_Id = $p0$)", thisOrderId);

            //if (thisAgentOrder != null)
            //{
            //    _isCashSale = thisAgentOrder.IsCashSale;
            //    _isRemittanceSale = thisAgentOrder.IsRemittanceSale;
            //    IsTradeTicketSale = thisAgentOrder.IsTradeTicketSale;
            //}

           // bool agentPurchase = false;

            //if (!string.IsNullOrEmpty(Request["ap"]))
            //{
            //    agentPurchase = Request["ap"].Trim() == "1" || Request["ap"].Trim() == "true";
            //}

            

            //if (agentPurchase)
            //{
            //    var printAgentTicket = false;

            //    if (Request["pat"] != null)
            //    {
            //        try
            //        {
            //            printAgentTicket = Convert.ToBoolean(Request["pat"]);
            //        }
            //        catch
            //        {
            //            printAgentTicket = false;
            //        }
            //    }

            //    if (printAgentTicket)
            //    {
            //        WriteMerchantReceiptDetails();
            //    }

            //    WriteCustomerReceiptDetails();
            //}

            var tourOrderLines = _allOrderLines.Where(a => a.TicketTorA == "Tour").ToList();

            var attractionOrderLines = _allOrderLines.Where(a => a.TicketTorA == "Attraction").ToList();

            //We need to group/filter by site, ticket and date
            var uniqueMicroSitesInTourOrderLineList =
                tourOrderLines.GroupBy(a => new { a.MicrositeId, a.TicketDate }).Select(x => new { x.Key.MicrositeId, x.Key.TicketDate }).ToList();

           
                if (tourOrderLines.Any())
                {
                    if (uniqueMicroSitesInTourOrderLineList.Count == 1)
                    {
                        //*** just check that QR Images exist UseOrderQrCode(_thisOrder);

                        var validDate = DateUtil.NullDate;
                        var ticketsitename = tourOrderLines[0].MicroSite.Name;

                        var validTicketname =
                            tourOrderLines[0].Ticket.Name.ToLower().Contains(ticketsitename.ToLower()) ?
                                tourOrderLines[0].Ticket.Name :
                                ticketsitename + " " + tourOrderLines[0].Ticket.Name;

                        var ticketline1 =
                            ((string.IsNullOrWhiteSpace(tourOrderLines[0].Ticket.TicketTextTopLine) ?
                                string.Empty :
                                tourOrderLines[0].Ticket.TicketTextTopLine.Trim() + " ") +
                             (string.IsNullOrWhiteSpace(tourOrderLines[0].Ticket.TicketTextMiddleLine) ?
                                 string.Empty :
                                 tourOrderLines[0].Ticket.TicketTextMiddleLine.Trim() + " ") +
                             (string.IsNullOrWhiteSpace(tourOrderLines[0].Ticket.TicketTextBottomLine) ?
                                 string.Empty :
                                 tourOrderLines[0].Ticket.TicketTextBottomLine.Trim() + " "))
                                .Trim();

                        var ticketline2 =
                            string.IsNullOrWhiteSpace(tourOrderLines[0].Ticket.TicketTextLine2) ?
                                string.Empty :
                                tourOrderLines[0].Ticket.TicketTextLine2.Trim();

                        var ticketline3 =
                            string.IsNullOrWhiteSpace(tourOrderLines[0].Ticket.TicketTextLine3) ?
                                string.Empty :
                                tourOrderLines[0].Ticket.TicketTextLine3.Trim();

                        var gdeparturet = string.Empty;
                        var gdeparturep = string.Empty;

                        var olist =
                            tourOrderLines.Where(
                                a =>
                                    a.TicketTorA == "Tour" &&
                                    a.TicketDate != DateUtil.NullDate &&
                                    a.TicketDate != new DateTime())
                                .ToList();

                        if (olist.Any())
                        {
                            var first = olist.OrderBy(a => a.TicketDate).FirstOrDefault();
                            
                            validDate = first.TicketDate ?? DateUtil.NullDate;

                            if (first.TicketType.ToLower().IndexOf("group") >= 0)
                            {
                                gdeparturet = first.DepartureTimeHour.PadLeft(2, '0') + ":" +
                                              first.DepartureTimeMinute.PadLeft(2, '0');
                                gdeparturep = first.DeparturePoint;
                            }

                            ticketsitename = first.MicroSite.Name;

                            validTicketname =
                                first.Ticket.Name.ToLower().Contains(ticketsitename.ToLower()) ?
                                    first.Ticket.Name :
                                    ticketsitename + " " + first.Ticket.Name;

                            ticketline1 =
                                ((string.IsNullOrWhiteSpace(first.Ticket.TicketTextTopLine) ?
                                    string.Empty :
                                    first.Ticket.TicketTextTopLine.Trim() + " ") +
                                 (string.IsNullOrWhiteSpace(first.Ticket.TicketTextMiddleLine) ?
                                     string.Empty :
                                     first.Ticket.TicketTextMiddleLine.Trim() + " ") +
                                 (string.IsNullOrWhiteSpace(first.Ticket.TicketTextBottomLine) ?
                                     string.Empty :
                                     first.Ticket.TicketTextBottomLine.Trim() + " ")).Trim();

                            ticketline2 =
                                string.IsNullOrWhiteSpace(first.Ticket.TicketTextLine2) ?
                                    string.Empty :
                                    first.Ticket.TicketTextLine2.Trim();

                            ticketline3 =
                                string.IsNullOrWhiteSpace(first.Ticket.TicketTextLine3) ?
                                    string.Empty :
                                    first.Ticket.TicketTextLine3.Trim();
                        }

                       // var imageMetaData = GetObjectFactory().GetObjectByOQL("*ImageMetaData(Name=$p0$)", "QRCODE 4 " + _thisOrder.OrderNumber) as IImageMetaData;

                        var newVoucherTicket = new VoucherTicket();

                        newVoucherTicket.MainDate =
                            validDate == DateUtil.NullDate ?
                                "Open day ticket" :
                                FormatDate(validDate);

                        newVoucherTicket.GroupDeparturePoint = gdeparturep;
                        newVoucherTicket.GroupDepartureTime = gdeparturet;

                        if (isPayPalTransaction)
                        {
                            newVoucherTicket.PaymentType = "PayPal";
                            newVoucherTicket.CcNumber = string.Empty;
                        }
                        else
                        {
                            newVoucherTicket.PaymentType = "CC number:";
                            newVoucherTicket.CcNumber =
                                !string.IsNullOrEmpty(_thisOrder.CcLast4Digits)
                                    ? "****" + _thisOrder.CcLast4Digits
                                    : string.Empty;
                        }

                        if (_isCashSale)
                        {
                            newVoucherTicket.PaymentType = _isRemittanceSale ? "REMIT" : "Cash Sale";
                            newVoucherTicket.CcNumber = string.Empty;

                            ticketline1 =
                                string.IsNullOrWhiteSpace(tourOrderLines[0].Ticket.TicketTextTopLine) ?
                                    string.Empty :
                                    tourOrderLines[0].Ticket.TicketTextTopLine.Trim() + " " +
                                    "Please bring proof of ID";
                        }

                        newVoucherTicket.CodeImageUrl =
                            imageMetaData != null ?
                                "/UploadedImages/" + imageMetaData.Image_Id + "." + imageMetaData.ImageType + "?w=200" :
                                string.Empty;

                        newVoucherTicket.TicketName = validTicketname;

                        if (IsTradeTicketSale && !string.IsNullOrWhiteSpace(_thisOrder.GiftTravelerName))
                        {
                            newVoucherTicket.LeadName = _thisOrder.GiftTravelerName;
                        }
                        else
                        {
                            if (isPayPalTransaction || string.IsNullOrWhiteSpace(_thisOrder.NameOnCard))
                            {
                                newVoucherTicket.LeadName = _thisOrder.UserName;
                            }
                            else
                            {
                                newVoucherTicket.LeadName = _thisOrder.NameOnCard;
                            }
                        }

                        newVoucherTicket.AgentRef =
                            string.IsNullOrWhiteSpace(_thisOrder.AgentRef) ?
                                string.Empty :
                                _thisOrder.AgentRef;

                        newVoucherTicket.OrderNumber = _thisOrder.OrderNumber.ToString();

                        var total = 0.0;

                        var adultQtylist = tourOrderLines.Where(x => x.TicketType.Equals("Adult")).ToList();
                        var childQtylist = tourOrderLines.Where(x => x.TicketType.Equals("Child")).ToList();
                        var familyQtylist = tourOrderLines.Where(x => x.TicketType.Equals("Family")).ToList();
                        var groupQtylist = tourOrderLines.Where(x => x.TicketType.Contains("Group")).ToList();

                        var aQuantity = adultQtylist.Sum(a => a.TicketQuantity);
                        var alineTotal = adultQtylist.Sum(a => a.GrossOrderLineValue);

                        newVoucherTicket.AdultQty = (aQuantity != null && adultQtylist.Any()) ? aQuantity.Value : 0;
                        total = (alineTotal != null && adultQtylist.Any()) ? alineTotal.Value : 0.0f;

                       

                        newVoucherTicket.ChildQty = childQtylist.Count() > 0 ? childQtylist.Sum(a => a.TicketQuantity) : 0;
                        total += childQtylist.Count() > 0 ? childQtylist.Sum(a => a.GrossOrderLineValue) : 0;

                       
                        newVoucherTicket.FamilyQty = familyQtylist.Count() > 0 ? familyQtylist.Sum(a => a.TicketQuantity) : 0;
                        total += familyQtylist.Count() > 0 ? familyQtylist.Sum(a => a.GrossOrderLineValue) : 0;

                       
                        newVoucherTicket.GroupQty = groupQtylist.Count() > 0 ? groupQtylist.Sum(a => a.TicketQuantity) : 0;
                        total += groupQtylist.Count() > 0 ? groupQtylist.Sum(a => a.GrossOrderLineValue) : 0;

                        newVoucherTicket.Price =
                            (_thisOrder.Currency != null ? _thisOrder.Currency.Symbol : string.Empty) +
                            total.ToString("0,0.00");

                        newVoucherTicket.OrderTotal =
                            (attractionOrderLines.Count() > 0 ?
                                ((_thisOrder.Currency != null ?
                                    _thisOrder.Currency.Symbol :
                                    string.Empty) +
                                 _thisOrder.Total.ToString("0,0.00")) :
                                string.Empty);

                        newVoucherTicket.TicketLine1 = ticketline1;
                        newVoucherTicket.TicketLine2 = ticketline2;
                        newVoucherTicket.TicketLine3 = ticketline3;
                        newVoucherTicket.GroupCQty = 0;
                        newVoucherTicket.GroupAQty = 0;
                        newVoucherTicket.ConcessionQty = 0;

                        MainList.Add(newVoucherTicket);
                    }
                    else
                    {
                        CreateBarcodeTickets(tourOrderLines, isPayPalTransaction);
                    }
                }

                if (attractionOrderLines.Any())
                {
                    var templist =
                        attractionOrderLines
                            .GroupBy(a => new { a.Ticket_Id, a.TicketDate, })
                            .Select(x => new { x.Key.Ticket_Id, x.Key.TicketDate })
                            .ToList();

                    foreach (var ticket in templist)
                    {
                        var ticket1 = ticket;
                        var innerlist = attractionOrderLines.Where(a => a.Ticket_Id == ticket1.Ticket_Id && a.TicketDate == ticket1.TicketDate).ToList();

                        if (innerlist.Any())
                        {
                            UseAttractionQrCode(_thisOrder, ticket1.Ticket_Id, ticket1.TicketDate);

                            var ticketsitename = innerlist[0].MicroSite.Name;

                            var validTicketname = innerlist[0].Ticket.Name.ToLower().Contains(ticketsitename.ToLower())
                                ? innerlist[0].Ticket.Name
                                : ticketsitename + " " + innerlist[0].Ticket.Name;

                            var ticketline1 =
                                ((string.IsNullOrWhiteSpace(innerlist[0].Ticket.TicketTextTopLine) ?
                                    string.Empty :
                                    innerlist[0].Ticket.TicketTextTopLine.Trim() + " ") +
                                 (string.IsNullOrWhiteSpace(innerlist[0].Ticket.TicketTextMiddleLine) ?
                                     string.Empty :
                                     innerlist[0].Ticket.TicketTextMiddleLine.Trim() + " ") +
                                 (string.IsNullOrWhiteSpace(innerlist[0].Ticket.TicketTextBottomLine) ?
                                     string.Empty :
                                     innerlist[0].Ticket.TicketTextBottomLine.Trim() + " "))
                                    .Trim();

                            var ticketline2 =
                                string.IsNullOrWhiteSpace(innerlist[0].Ticket.TicketTextLine2) ?
                                    string.Empty :
                                    innerlist[0].Ticket.TicketTextLine2.Trim();

                            var ticketline3 =
                                string.IsNullOrWhiteSpace(innerlist[0].Ticket.TicketTextLine3) ?
                                    string.Empty :
                                    innerlist[0].Ticket.TicketTextLine3.Trim();

                            var gdeparturet = string.Empty;
                            var gdeparturep = string.Empty;

                            var vt =
                                new VoucherTicket
                                {
                                    MainDate = FormatDate(ticket.TicketDate),
                                    CodeImageUrl =
                                        "/QRBARCodes/QRCodes/" +
                                        _thisOrder.OrderNumber +
                                        ticket1.Ticket_Id +
                                        ticket1.TicketDate.ToString("ddMMyyyy") +
                                        ".png",
                                    TicketName = validTicketname,
                                    LeadName = _thisOrder.NameOnCard,
                                    AttractionImageUrl = GetMetaDataImageUrl(innerlist[0].Ticket),
                                    OrderNumber = _thisOrder.OrderNumber.ToString(),
                                    TicketLine1 = ticketline1,
                                    TicketLine2 = ticketline2,
                                    TicketLine3 = ticketline3,
                                    IsAttraction = true
                                };

                            if (isPayPalTransaction)
                            {
                                vt.PaymentType = "PayPal";
                                vt.CcNumber = string.Empty;
                            }
                            else
                            {
                                vt.PaymentType = "CC number:";
                                vt.CcNumber =
                                    !string.IsNullOrEmpty(_thisOrder.CCLast4Digits)
                                        ? "****" + _thisOrder.CCLast4Digits
                                        : string.Empty;
                            }

                            if (_isCashSale)
                            {
                                vt.PaymentType = _isRemittanceSale ? "REMIT" : "Cash Sale";
                                vt.CcNumber = string.Empty;
                            }

                            var total = new decimal(0);

                            var adultQtylist = innerlist.Where(x => x.TicketType.Equals("Adult")).ToList();

                            vt.AdultQty = adultQtylist.Count() > 0 ? adultQtylist.Sum(a => a.TicketQuantity) : 0;
                            total += adultQtylist.Count() > 0 ? adultQtylist.Sum(a => a.GrossOrderLineValue) : 0;

                            var childQtylist = innerlist.Where(x => x.TicketType.Equals("Child")).ToList();

                            vt.ChildQty = childQtylist.Count() > 0 ? childQtylist.Sum(a => a.TicketQuantity) : 0;
                            total += childQtylist.Count() > 0 ? childQtylist.Sum(a => a.GrossOrderLineValue) : 0;

                            var familyQtylist = innerlist.Where(x => x.TicketType.Equals("Family")).ToList();

                            vt.FamilyQty = familyQtylist.Count() > 0 ? familyQtylist.Sum(a => a.TicketQuantity) : 0;
                            total += familyQtylist.Count() > 0 ? familyQtylist.Sum(a => a.GrossOrderLineValue) : 0;

                            var concQtylist = innerlist.Where(x => x.TicketType.Equals("Concession")).ToList();

                            vt.ConcessionQty = concQtylist.Count() > 0 ? concQtylist.Sum(a => a.TicketQuantity) : 0;
                            total += concQtylist.Count() > 0 ? concQtylist.Sum(a => a.GrossOrderLineValue) : 0;

                            vt.Price = (_thisOrder.Currency != null ? _thisOrder.Currency.Symbol : string.Empty) + total.ToString("0,0.00");

                            vt.GroupCQty = 0;
                            vt.GroupAQty = 0;
                            vt.GroupQty = 0;

                            MainList.Add(vt);
                        }
                    }
                }
           
        }

        public struct VoucherTicket
        {
            public string TicketName { get; set; }

            public string MainDate { get; set; }

            public int AdultQty { get; set; }

            public int ChildQty { get; set; }

            public int FamilyQty { get; set; }

            public int ConcessionQty { get; set; }

            public int GroupQty { get; set; }

            public string Barcode { get; set; }

            public string LeadName { get; set; }

            public string PaymentType { get; set; }

            public string CcNumber { get; set; }

            public string AttractionImageUrl { get; set; }

            public string CodeImageUrl { get; set; }

            public string GroupDepartureTime { get; set; }

            public string GroupDeparturePoint { get; set; }

            public int GroupAQty { get; set; }

            public int GroupCQty { get; set; }

            public string OrderNumber { get; set; }

            public string AgentRef { get; set; }

            public string Price { get; set; }

            public string OrderTotal { get; set; }

            public string TicketLine1 { get; set; }

            public string TicketLine2 { get; set; }

            public string TicketLine3 { get; set; }

            public bool IsAttraction { get; set; }
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

        private string GetMetaDataImageUrl(Ticket thisTicket)
        {
            if (!string.IsNullOrEmpty(thisTicket.TicketImageMetaData_Id) && thisTicket.TicketImageEnabled)
            {
                ImageMetaData thisImageMetaData = null;

                try
                {
                    thisImageMetaData = GetObjectFactory().GetById<ImageMetaData>(thisTicket.TicketImageMetaData_Id);
                }
                catch
                {
                }

                return
                    thisImageMetaData != null ?
                        "/UploadedImages/" + thisImageMetaData.Image_Id + "." + thisImageMetaData.ImageType + "?h=102&w=124" :
                        string.Empty;
            }

            return string.Empty;
        }

        private void MakeSureBarcodesExist(OrderLine thisOrderLine)
        {
            var thisOg = thisOrderLine.GetBarcodes();
            var agentprefix = string.IsNullOrWhiteSpace(thisOrderLine.Order.AgentRef) ? string.Empty : "agent ";

            if (thisOg != null && thisOg.Count != 0) return;

            for (var y = 0; y < thisOrderLine.TicketQuantity; y++)
            {
                //Retrieve a new the next available barcode for this tickets type
                var barcode = GetNextBarcode(thisOrderLine.Ticket, thisOrderLine.TicketType, agentprefix).Substring(0, 12);
                var thisOrderLineGeneratedBarcode = GetObjectFactory().GetBlankNew<OrderLine_GeneratedBarcode>();

                thisOrderLineGeneratedBarcode.OrderLine_Id = thisOrderLine.Id;
                thisOrderLineGeneratedBarcode.GeneratedBarcode = barcode + CalculateBarcodeChecksum(barcode.Substring(0, 12));
                thisOrderLineGeneratedBarcode.PersistDataAsNew();
            }
        }

        private string GetBarCodeImageUrl(string barcode)
        {
            var fi = new FileInfo(Server.MapPath("~/QRBARCodes/BarCodes/" + barcode + ".jpg"));

            if (!fi.Exists)
            {
                try
                {
                    var generatedBarcodeBitmap = new System.Drawing.Bitmap(105, 65);

                    //create a new graphic using the bitmap
                    var barcodeGraphic = System.Drawing.Graphics.FromImage(generatedBarcodeBitmap);

                    //ensure that the background is white
                    barcodeGraphic.Clear(System.Drawing.Color.White);
                    //draw the barcode image on the graphic
                    barcodeGraphic.DrawImage(
                        Zen.Barcode.BarcodeDrawFactory.CodeEan13WithChecksum.Draw(barcode.Substring(0, 12), 40), 5, 5);
                    barcodeGraphic.Flush();


                    generatedBarcodeBitmap.Save(fi.FullName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    generatedBarcodeBitmap.Dispose();
                }
                catch
                {
                }
            }

            return "/QRBARCodes/BarCodes/" + barcode + ".jpg";
        }

        private void WriteMerchantReceiptDetails()
        {
            var ticketContent = new System.Text.StringBuilder();

            if (_allOrderLines.Any())
            {
                var receiptHeader = "<span style=\"font-size:24px;font-weight:Bold;\">" + GetTranslation("agt_MerchantReceipt") + "</span>\n\n\n";

                //TODO: Add Agent details
                ticketContent.AppendFormat("{0} {1} \n\n", _thisSession.User.Firstname.ToUpperInvariant(), _thisSession.User.Lastname.ToUpperInvariant());
                // ticketContent.AppendFormat("{0} {1} \n\n",GetTranslation("agt_AgentRef"), ThisSession.User.AgentProfile_Id);

                PopulateOrderDetailsForReceipt(ticketContent);

                if (ticketContent.Length > 0)
                {
                    ticketContent.Insert(0, "<span style=\"font-size:18px;font-weight:Bold;\">");

                    ticketContent.Insert(0, receiptHeader);
                    ticketContent.Append("</span>");

                    LiMerchantReceipt.Text = "<div style=\"font-size:18px;padding_top:40px;\">" + GeneralHelpers.Parsetext(ticketContent.ToString(), true) + "</div><div class=\"page-breaker\">&nbsp;</div>";
                }
            }
        }

        private void WriteCustomerReceiptDetails()
        {
            var ticketContent = new System.Text.StringBuilder();

            if (_allOrderLines.Any())
            {
                var receiptHeader = "<span style=\"font-size:24px;font-weight:Bold;\">" + GetTranslation("agt_CustomerReceipt") + "</span>\n\n\n";

                PopulateOrderDetailsForReceipt(ticketContent);

                ticketContent.AppendFormat("\n\n{0}", GetTranslation("agt_Please_PresentphotoId"));

                //enjoy your trip text
                ticketContent.AppendFormat("\n\n{0}\n ", GetTranslation("email_Enjoy_your_trip"));

                //If you have any queries
                ticketContent.AppendFormat("\n{0}\n\n ", GetTranslation("email_If you_have_any_queries"));

                //site address
                ticketContent.AppendFormat("\n{0}\n\n ", MicrositeAddressInfo.GetAddressInfo(CurrentMicroSite.Id, this.TranslationServices));

                //please present
                //  ticketContent.AppendFormat("\n{0}\n\n ", GetTranslation("agt_PleasePresentId"));

                if (ticketContent.Length > 0)
                {
                    ticketContent.Insert(0, "<span style=\"font-size:18px;font-weight:Bold;\">");

                    ticketContent.Insert(0, receiptHeader);
                    ticketContent.Append("</span>");

                    LiCustomerReceipt.Text = "<div style=\"font-size:18px;padding_top:40px;\">" + GeneralHelpers.Parsetext(ticketContent.ToString(), true) + "</div><div class=\"page-breaker\">&nbsp;</div>";
                }
            }
        }

        private void PopulateOrderDetailsForReceipt(System.Text.StringBuilder ticketContent)
        {
            const string orderLinesformat = "\n\n     {0}  {1} {2}{3}";

            ticketContent.AppendFormat("{0} {1} \n\n", GetTranslation("agt_AgentRef"), _thisSession.User.AgentProfile_Id);

            ticketContent.Append(GetTranslation("agt_OrderDetails") + "    -     " + CityDateTime.Now.ToString("dd MMMM yyyy HH:mm:ss"));

            //Display Order ref
            ticketContent.AppendFormat("\n\n     {0} : {1}", GetTranslation("agt_OrderRef"), _thisOrder.OrderNumber);

            //Display cusomter name
            if (string.IsNullOrWhiteSpace(_thisOrder.User.Firstname) ||
                string.IsNullOrWhiteSpace(_thisOrder.User.Lastname))
            {
                ticketContent.AppendFormat("\n\n     {0}", _thisOrder.NameOnCard);
            }
            else
            {
                ticketContent.AppendFormat("\n\n     {0} {1}", _thisOrder.User.Firstname.ToUpperInvariant(), _thisOrder.User.Lastname.ToUpperInvariant());
            }

            // Display line items list
            foreach (OrderLine bLine in _allOrderLines)
            {
                var ticketCost = bLine.TicketCost;
                ticketContent.AppendFormat(orderLinesformat, bLine.TicketQuantity, bLine.TicketType, _thisSession.Currency.Symbol, (ticketCost * bLine.TicketQuantity));
            }

            //Display Total
            ticketContent.AppendFormat("\n\n     {0} : {1}", GetTranslation("Total"), _thisSession.Currency.Symbol + " " + _thisOrder.Total);
        }

        private void CreateBarcodeTickets(IEnumerable<OrderLine> olines, bool isPayPalTransaction)
        {
            foreach (var orderLine in olines)
            {
                MakeSureBarcodesExist(orderLine);

                var barcodes = orderLine.GetBarcodes();

                foreach (OrderLine_GeneratedBarcode olgb in barcodes)
                {
                    var validDate = orderLine.TicketDate;
                    var ticketsitename = orderLine.MicroSite.Name;

                    var validTicketname =
                        orderLine.Ticket.Name.ToLower().Contains(ticketsitename.ToLower()) ?
                            orderLine.Ticket.Name :
                            ticketsitename + " " + orderLine.Ticket.Name;

                    var ticketline1 =
                        ((string.IsNullOrWhiteSpace(orderLine.Ticket.TicketTextTopLine) ?
                            string.Empty :
                            orderLine.Ticket.TicketTextTopLine.Trim() + " ") +
                        (string.IsNullOrWhiteSpace(orderLine.Ticket.TicketTextMiddleLine) ?
                            string.Empty :
                            orderLine.Ticket.TicketTextMiddleLine.Trim() + " ") +
                        (string.IsNullOrWhiteSpace(orderLine.Ticket.TicketTextBottomLine) ?
                            string.Empty :
                            orderLine.Ticket.TicketTextBottomLine.Trim() + " ")).Trim();

                    var ticketline2 =
                        string.IsNullOrWhiteSpace(orderLine.Ticket.TicketTextLine2) ?
                            string.Empty :
                            orderLine.Ticket.TicketTextLine2.Trim();

                    var ticketline3 =
                        string.IsNullOrWhiteSpace(orderLine.Ticket.TicketTextLine3) ?
                            string.Empty :
                            orderLine.Ticket.TicketTextLine3.Trim();

                    var gdeparturet = string.Empty;
                    var gdeparturep = string.Empty;

                    if (orderLine.TicketType.ToLower().IndexOf("group") >= 0)
                    {
                        gdeparturet =
                            orderLine.DepartureTimeHour.PadLeft(2, '0') + ":" + orderLine.DepartureTimeMinute.PadLeft(2, '0');

                        gdeparturep = orderLine.DeparturePoint;
                    }

                    var vt = new VoucherTicket
                    {
                        MainDate =
                            validDate == GetObjectFactory().NullDate ?
                                "Open day ticket" :
                                FormatDate(validDate),

                        GroupDeparturePoint = gdeparturep,
                        GroupDepartureTime = gdeparturet,

                        CcNumber =
                            !string.IsNullOrEmpty(_thisOrder.CCLast4Digits) ?
                                "****" + _thisOrder.CCLast4Digits :
                                string.Empty,

                        CodeImageUrl = GetBarCodeImageUrl(olgb.GeneratedBarcode),

                        TicketName = validTicketname,
                        LeadName = _thisOrder.NameOnCard,

                        AgentRef =
                            string.IsNullOrWhiteSpace(_thisOrder.AgentRef) ?
                                string.Empty :
                                _thisOrder.AgentRef,

                        AdultQty =
                            orderLine.TicketType.Equals("Adult") ? 1 : 0,

                        ChildQty =
                            orderLine.TicketType.Equals("Child") ? 1 : 0,

                        FamilyQty =
                            orderLine.TicketType.Equals("Family") ? 1 : 0,

                        ConcessionQty =
                            orderLine.TicketType.Equals("Concession") ? 1 : 0,

                        GroupAQty =
                            orderLine.TicketType.Equals("GroupAdult") ? 1 : 0,

                        GroupCQty =
                            orderLine.TicketType.Equals("GroupChild") ? 1 : 0,

                        AttractionImageUrl = GetMetaDataImageUrl(orderLine.Ticket),
                        Barcode = olgb.GeneratedBarcode,
                        OrderNumber = _thisOrder.OrderNumber.ToString(),
                        GroupQty = 0,
                        Price =
                            (_thisOrder.Currency != null ? _thisOrder.Currency.Symbol : string.Empty) +
                            orderLine.TicketCost.ToString("0,0.00"),

                        TicketLine1 = ticketline1,
                        TicketLine2 = ticketline2,
                        TicketLine3 = ticketline3,
                        IsAttraction = orderLine.TicketTOrA.ToLower().Equals("attraction")
                    };

                    if (isPayPalTransaction)
                    {
                        vt.PaymentType = "PayPal";
                        vt.CcNumber = string.Empty;
                    }
                    else
                    {
                        vt.PaymentType = "CC number:";
                        vt.CcNumber =
                            !string.IsNullOrEmpty(_thisOrder.CCLast4Digits)
                                ? "****" + _thisOrder.CCLast4Digits
                                : string.Empty;
                    }

                    if (_isCashSale)
                    {
                        vt.PaymentType = this._isRemittanceSale ? "REMIT" : "Cash Sale";
                        vt.CcNumber = string.Empty;
                    }

                    MainList.Add(vt);
                }
            }
        }
    }

}
