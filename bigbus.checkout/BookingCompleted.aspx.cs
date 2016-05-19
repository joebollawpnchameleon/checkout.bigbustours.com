using bigbus.checkout.data.Model;
using bigbus.checkout.Helpers;
using bigbus.checkout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace bigbus.checkout
{
    public partial class BookingCompleted : BasePage
    {
        protected string UserEmail { get; set; }
        protected string IntileryTagScript { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            lbError.Visible = false;
            var orderId = Request.QueryString["oid"];
            if (string.IsNullOrEmpty(orderId))
                return;

            var order = CheckoutService.GetFullOrder(orderId);

            if (order == null)
            {
                lbError.Text = "Invalid order request.";//*** translation needed
                lbError.Visible = true;
                Log("Invalid order request orderid: " + orderId);
                return;
            }

            TrackOrderCompleted(order);
        }
        

        private void TrackOrderCompleted(Order order)
        {
            try
            {
                var orderLines = order.OrderLines;
                var temp = orderLines.Select(x => x.MicrositeId).ToList();
                var shippingCities = string.Join(",", temp.ToArray());
                var allScripts = new StringBuilder();


                var transactionOrder = new IntileryTagHelper.TransactionOrder
                {
                    OrderNumber = order.OrderNumber.ToString(),
                    OrderTax = (decimal)0.0,
                    OrderTotal = order.Total.Value,
                    ShippingCity = shippingCities,
                    StoreName = "BigBusTours",
                    TransactionCurrency = order.Currency.ISOCode
                };

                if (order.User != null)
                {
                    var language = TranslationService.GetLanguage(order.LanguageId);

                    transactionOrder.Customer = new IntileryTagHelper.TransactionCustomer
                    {
                        FirstName = order.User.Firstname,
                        LastName = order.User.Lastname,
                        Email = order.User.FriendlyEmail,
                        Language = language.ShortCode,
                        Address1 = order.User.AddressLine1,
                        Address2 = order.User.AddressLine2,
                        Town = order.User.City,
                        PostCode = order.User.PostCode,
                        Country = order.User.CountryId
                    };
                }

                allScripts.AppendLine(IntileryTagHelper.TrackAddTrans(transactionOrder));

                //add orderlines
                foreach (var line in orderLines)
                {
                    var transactionItem = new IntileryTagHelper.TransactionOrderItem
                    {
                        OrderNumber = order.OrderNumber.ToString(),
                        ItemCategory = line.TicketTorA,
                        ItemName = line.Ticket.Name,
                        ItemPrice = line.TicketCost.Value,
                        ItemQuantity = line.TicketQuantity.Value,
                        ItemSku = line.TicketId.ToString(),
                        ProductItem = new IntileryTagHelper.ProductBasketLine
                        {
                            ProductId = line.TicketId.ToString(),
                            LineTotal = line.NettOrderLineValue.ToString(),
                            PassengerType = line.TicketType,
                            ProductLanguage = CurrentLanguageId,
                            ProductName = line.Ticket.Name,
                            ProductSupplierCode = line.TicketId.ToString(),
                            Quantity = line.TicketQuantity.Value,
                            TicketType = line.TicketTorA
                        }
                    };

                    allScripts.AppendLine(IntileryTagHelper.TrackAddTransItem(transactionItem));
                }

                IntileryTagScript = allScripts.ToString();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }


    }
}