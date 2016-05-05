//using System;
//using System.Collections.Generic;
//using System.Linq;
//using bigbus.checkout.Models;
//using bigbus.checkout.data.Model;
//using Services.Infrastructure;

using bigbus.checkout.Models;

namespace bigbus.checkout
{
    public partial class Voucher : BasePage
    {
        //        public ICheckoutService CheckoutService { get; set; }

        //        private Order _thisOrder;
        //        private List<OrderLine> _allOrderLines;
        //        private Session _thisSession;
        //        private bool _isCashSale;
        //        private bool _isRemittanceSale;

        //        //private EVoucherCreationHelper _voucherHelper;

        //        public bool IsTradeTicketSale;

        //        //public List<VoucherTicket> MainList = new List<VoucherTicket>();
        //        //protected VoucherBarcodeBusinessRule VoucherBusinessRule;
        //        protected string EcrBookingRef { get; set; }

        //        protected void Page_Load(object sender, EventArgs e)
        //        {
        //            //EnsureSessionCorrectlyEstablished(); // uses virtual function so logic can be overridden

        //            //EstablishURL();

        //            if (!string.IsNullOrEmpty(Request["Id"]))
        //            {
        //                var orderId = Request["Id"];

        //                _thisOrder = CheckoutService.GetFullOrder(orderId);

        //                if (_thisOrder != null)
        //                {
        //                    EcrBookingRef = _thisOrder.EcrBookingShortReference;

        //                    VoucherBusinessRule = new VoucherBarcodeBusinessRule(_thisOrder);

        //                    var isPayPalTransaction = _thisOrder.PaymentMethod.Equals("PAYPAL", StringComparison.OrdinalIgnoreCase);

        //                    if (!string.IsNullOrWhiteSpace(Request["sid"]))
        //                    {
        //                        _thisSession = GetObjectFactory().GetByOQL<Session>("*(1)Session(Id = $p0$)", Request["sid"]);
        //                    }

        //                    var thisAgentOrder = GetObjectFactory().GetByOQL<AgentOrder>("?(*)AgentOrder(Order_Id = $p0$)", thisOrderId);

        //                    if (thisAgentOrder != null)
        //                    {
        //                        _isCashSale = thisAgentOrder.IsCashSale;
        //                        _isRemittanceSale = thisAgentOrder.IsRemittanceSale;
        //                        IsTradeTicketSale = thisAgentOrder.IsTradeTicketSale;
        //                    }

        //                    bool agentPurchase = false;

        //                    if (!string.IsNullOrEmpty(Request["ap"]))
        //                    {
        //                        agentPurchase = Request["ap"].Trim() == "1" || Request["ap"].Trim() == "true";
        //                    }

        //                    _allOrderLines = new List<OrderLine>(_thisOrder.GetOrderLines());

        //                    if (agentPurchase)
        //                    {
        //                        var printAgentTicket = false;

        //                        if (Request["pat"] != null)
        //                        {
        //                            try
        //                            {
        //                                printAgentTicket = Convert.ToBoolean(Request["pat"]);
        //                            }
        //                            catch
        //                            {
        //                                printAgentTicket = false;
        //                            }
        //                        }

        //                        if (printAgentTicket)
        //                        {
        //                            WriteMerchantReceiptDetails();
        //                        }

        //                        WriteCustomerReceiptDetails();
        //                    }

        //                    _voucherHelper = new EVoucherCreationHelper(this, _thisOrder, _isCashSale, _isRemittanceSale, IsTradeTicketSale);

        //                    if (CurrentMicroSite.UseQr)
        //                    {
        //                        AddVoucherTicketList(_voucherHelper.GetTourVoucherTickets());
        //                        AddVoucherTicketList(_voucherHelper.GetComboVoucherTickets());
        //                        AddVoucherTicketList(_voucherHelper.GetAllTimedAttractionVoucherTickets());
        //                        AddVoucherTicketList(_voucherHelper.GetAttractionVoucherTickets());
        //                    }
        //                    else
        //                    {
        //                        AddVoucherTicketList(_voucherHelper.GetAllVoucherTicketWithBarcodes());
        //                    }
        //                }
        //            }
        //        }

        //        private void AddVoucherTicketList(List<VoucherTicket> lstVoucherTickets)
        //        {
        //            if (lstVoucherTickets != null && lstVoucherTickets.Count > 0)
        //                MainList.AddRange(lstVoucherTickets);
        //        }

        //        private void WriteMerchantReceiptDetails()
        //        {
        //            var ticketContent = new System.Text.StringBuilder();

        //            if (_allOrderLines.Any())
        //            {
        //                var receiptHeader = "<span style=\"font-size:24px;font-weight:Bold;\">" + GetTranslation("agt_MerchantReceipt") + "</span>\n\n\n";

        //                //TODO: Add Agent details
        //                ticketContent.AppendFormat("{0} {1} \n\n", _thisSession.User.Firstname.ToUpperInvariant(), _thisSession.User.Lastname.ToUpperInvariant());
        //                // ticketContent.AppendFormat("{0} {1} \n\n",GetTranslation("agt_AgentRef"), ThisSession.User.AgentProfile_Id);

        //                PopulateOrderDetailsForReceipt(ticketContent);

        //                if (ticketContent.Length > 0)
        //                {
        //                    ticketContent.Insert(0, "<span style=\"font-size:18px;font-weight:Bold;\">");

        //                    ticketContent.Insert(0, receiptHeader);
        //                    ticketContent.Append("</span>");

        //                    LiMerchantReceipt.Text = "<div style=\"font-size:18px;padding_top:40px;\">" + GeneralHelpers.Parsetext(ticketContent.ToString(), true) + "</div><div class=\"page-breaker\">&nbsp;</div>";
        //                }
        //            }
        //        }

        //        private void WriteCustomerReceiptDetails()
        //        {
        //            var ticketContent = new System.Text.StringBuilder();

        //            if (_allOrderLines.Any())
        //            {
        //                var receiptHeader = "<span style=\"font-size:24px;font-weight:Bold;\">" + GetTranslation("agt_CustomerReceipt") + "</span>\n\n\n";

        //                PopulateOrderDetailsForReceipt(ticketContent);

        //                ticketContent.AppendFormat("\n\n{0}", GetTranslation("agt_Please_PresentphotoId"));

        //                //enjoy your trip text
        //                ticketContent.AppendFormat("\n\n{0}\n ", GetTranslation("email_Enjoy_your_trip"));

        //                //If you have any queries
        //                ticketContent.AppendFormat("\n{0}\n\n ", GetTranslation("email_If you_have_any_queries"));

        //                //site address
        //                ticketContent.AppendFormat("\n{0}\n\n ", MicrositeAddressInfo.GetAddressInfo(CurrentMicroSite.Id, this.TranslationServices));

        //                //please present
        //                //  ticketContent.AppendFormat("\n{0}\n\n ", GetTranslation("agt_PleasePresentId"));

        //                if (ticketContent.Length > 0)
        //                {
        //                    ticketContent.Insert(0, "<span style=\"font-size:18px;font-weight:Bold;\">");

        //                    ticketContent.Insert(0, receiptHeader);
        //                    ticketContent.Append("</span>");

        //                    LiCustomerReceipt.Text = "<div style=\"font-size:18px;padding_top:40px;\">" + GeneralHelpers.Parsetext(ticketContent.ToString(), true) + "</div><div class=\"page-breaker\">&nbsp;</div>";
        //                }
        //            }
        //        }

        //        private void PopulateOrderDetailsForReceipt(System.Text.StringBuilder ticketContent)
        //        {
        //            const string orderLinesformat = "\n\n     {0}  {1} {2}{3}";

        //            ticketContent.AppendFormat("{0} {1} \n\n", GetTranslation("agt_AgentRef"), _thisSession.User.AgentProfile_Id);

        //            ticketContent.Append(GetTranslation("agt_OrderDetails") + "    -     " + CityDateTime.Now.ToString("dd MMMM yyyy HH:mm:ss"));

        //            //Display Order ref
        //            ticketContent.AppendFormat("\n\n     {0} : {1}", GetTranslation("agt_OrderRef"), _thisOrder.OrderNumber);

        //            //Display cusomter name
        //            if (string.IsNullOrWhiteSpace(_thisOrder.User.Firstname) ||
        //                string.IsNullOrWhiteSpace(_thisOrder.User.Lastname))
        //            {
        //                ticketContent.AppendFormat("\n\n     {0}", _thisOrder.NameOnCard);
        //            }
        //            else
        //            {
        //                ticketContent.AppendFormat("\n\n     {0} {1}", _thisOrder.User.Firstname.ToUpperInvariant(), _thisOrder.User.Lastname.ToUpperInvariant());
        //            }

        //            // Display line items list
        //            foreach (OrderLine bLine in _allOrderLines)
        //            {
        //                var ticketCost = bLine.TicketCost;
        //                ticketContent.AppendFormat(orderLinesformat, bLine.TicketQuantity, bLine.TicketType, _thisSession.Currency.Symbol, (ticketCost * bLine.TicketQuantity));
        //            }

        //            //Display Total
        //            ticketContent.AppendFormat("\n\n     {0} : {1}", GetTranslation("Total"), _thisSession.Currency.Symbol + " " + _thisOrder.Total);
        //        }

    }

}
