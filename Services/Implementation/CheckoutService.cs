
using System;
using System.Linq;
using bigbus.checkout.data.Model;
using pci = Common.Model.Pci;
using Services.Infrastructure;
using Basket = bigbus.checkout.data.Model.Basket;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Common.Model;

namespace Services.Implementation
{
    public class CheckoutService : BaseService, ICheckoutService
    {
        public virtual Order CreateOrder(Session session, Basket basket, pci.BasketStatus basketStatus, string clientIpAddress, string languageId, string micrositeId)
        {
            try
            {
                Guid? currencyId =  new Guid(session.CurrencyId);
                var currency = CurrencyRepository.GetSingle(x => x.Id.Equals(currencyId));
                var user = UserRepository.GetSingle(x => x.Id == basket.UserId);

                var order = new Order
                {
                    PaymentMethod = basketStatus.card.type,
                    NumbViewPdf = 0,
                    OpenForPrinting = true,
                    DatePdfLastViewed = DateTime.Now,
                    UserId = user.Id,
                    UserName = user.Firstname + " " + user.Lastname,
                    EmailAddress = string.IsNullOrWhiteSpace(user.FriendlyEmail) ? user.Email : user.FriendlyEmail,
                    CurrencyId = currencyId,
                    LanguageId = languageId,
                    Total = basket.Total,
                    CcLast4Digits = basketStatus.card.number,
                    ClientIp = clientIpAddress,
                    WorldPayMerchantId = basketStatus.merchant.merchantId,
                    GatewayReference = basketStatus.merchant.gatewayref,
                    NameOnCard = basketStatus.card.name,
                    BasketId = basket.Id,
                    SessionId = session.Id,
                    TotalQuantity = basket.BasketLines.Sum(x => x.TicketQuantity),
                    IsMobileAppOrder = false, //check how this is populated on old system.
                    DateCreated = LocalizationService.GetLocalDateTime(micrositeId),
                    FromNewCheckout = true
                };

                //populate order lines with existing baskets.
                foreach (var basketLine in basket.BasketLines)
                {
                    order.OrderLines.Add(ConvertBasketLineToOrderLine(basketLine, order.Id));
                }

                OrderRepository.Add(order);

                order.User = user;
                order.Currency = currency;

                return order;
            }
            catch (Exception ex)
            {
                Log(string.Format("Order Create Failed sessionid: {0} basketid:{1} {2} ", session.Id, basket.Id, ex.Message));
                return null;
            }
           
        }

        public virtual Order CreateOrderPayPal(Session session, Basket basket, User user, string clientIpAddress, string languageId, string micrositeId)
        {
            try
            {
                Guid? currencyId = new Guid(session.CurrencyId);
                var currency = CurrencyRepository.GetSingle(x => x.Id.Equals(currencyId));
               
                var order = new Order
                {
                    PaymentMethod = "PayPal",
                    NumbViewPdf = 0,
                    OpenForPrinting = true,
                    DatePdfLastViewed = DateTime.Now,
                    UserId = user.Id,
                    UserName = user.Firstname + " " + user.Lastname,
                    EmailAddress = string.IsNullOrWhiteSpace(user.FriendlyEmail) ? user.Email : user.FriendlyEmail,
                    CurrencyId = currencyId,
                    LanguageId = languageId,
                    Total = basket.Total,
                    ClientIp = clientIpAddress,
                    BasketId = basket.Id,
                    SessionId = session.Id,
                    TotalQuantity = basket.BasketLines.Sum(x => x.TicketQuantity),
                    IsMobileAppOrder = false, //check how this is populated on old system.
                    DateCreated = LocalizationService.GetLocalDateTime(micrositeId),
                    PayPalOrderId = session.PayPalOrderId,
                    PayPalPayerId = session.PayPalPayerId,
                    PayPalToken = session.PayPalToken,
                    FromNewCheckout = true
                };

                //populate order lines with existing baskets.
                foreach (var basketLine in basket.BasketLines)
                {
                    order.OrderLines.Add(ConvertBasketLineToOrderLine(basketLine, order.Id));
                }

                OrderRepository.Add(order);

                order.User = user;
                order.Currency = currency;

                return order;
            }
            catch (Exception ex)
            {
                Log(string.Format("Paypal Order Create Failed sessionid: {0} basketid:{1} {2} ", session.Id, basket.Id, ex.Message));
                return null;
            }  
        }

        public virtual TransactionAddressPaypal CreateAddressPaypal(Order order, Session session, User user)
        {
            var payPalAddress = new TransactionAddressPaypal
            {
                OrderId = order.Id.ToString(),
                OrderNumber = order.OrderNumber,
                Email = user.Email,
                Title = user.Title,
                BillToFirstName = user.Firstname,
                BillToLastName = user.Lastname,

                //Address broken up and combined to make street
                BillToAddress1 = user.AddressLine1,
                BillToAddress2 = user.AddressLine2,
                BillToStreet = string.Concat(user.AddressLine1, " ", user.AddressLine2),

                BillToCity = user.City,
                BillToPostCode = user.PostCode,
                BillToState = user.StateProvince,
                BillToCountry = user.CountryId,

                //Shipping to information
                ShipToFirstName = user.Firstname,
                ShipToLastName = user.Lastname,

                //Address broken up and combined to make street
                ShipToAddress1 = user.AddressLine1,
                ShipToAddress2 = user.AddressLine2,
                ShipToStreet = string.Concat(user.AddressLine1, " ", user.AddressLine2),

                ShipToCity = user.City,
                ShipToPostCode = user.PostCode,
                ShipToState = user.StateProvince,
                ShipToCountry = user.CountryId,

                SessionId = session.Id.ToString(),
                UserId = session.UserId.ToString(),
                PayPalOrderId = session.PayPalOrderId,
                PayPalToken = session.PayPalToken,
                PayPalPayerId = session.PayPalPayerId,
                BasketId = session.BasketId.ToString(),
            };

            AddressPpRepository.Add(payPalAddress);

            return payPalAddress;
        }

        public virtual OrderLine ConvertBasketLineToOrderLine(BasketLine basketLine, Guid orderId)
        {
            Log(string.Format("Entering CheckoutService - ConvertBasketLineToOrderLine() orderid:{0} - basketlineid {1}", orderId, basketLine.Id));

            var basketLineTicket =TicketRepository.GetSingle(x => x.Id == basketLine.TicketId);

            var orderLine = new OrderLine
            {
                OrderId = orderId,
                TicketId = basketLine.TicketId,
                FixedDateTicket = basketLine.FixedDateTicket,
                TicketDate = basketLine.TicketDate,
                TicketType = basketLine.TicketType,
                TicketQuantity = basketLine.TicketQuantity,
                PromotionId = basketLine.PromotionId,
                DepartureTimeHour = basketLine.DepartureTimeHour,
                DepartureTimeMinute = basketLine.DepartureTimeMinute,
                DeparturePoint = basketLine.DeparturePoint,
                TicketTorA = basketLineTicket.TicketType,
                MicrositeId = basketLineTicket.MicroSiteId,
                TicketCost = basketLine.Price,
                GrossOrderLineValue = basketLine.LineTotal + basketLine.Discount,
                NettOrderLineValue = basketLine.LineTotal,
                EcrProductDimensionId = basketLine.EcrProductDimensionId,
                ExternalCoupon = basketLine.ExternalCoupon,
                ExternalOrder = basketLine.ExternalOrder
            };

            if (basketLineTicket.TicketType.Equals("attraction", StringComparison.CurrentCultureIgnoreCase))
            {
                orderLine.AttractionName = basketLineTicket.Name;
            }
           
            return orderLine;
        }

        public virtual void SaveOrder(Order order)
        {
            OrderRepository.Update(order);
        }

        public List<OrderLineGeneratedBarcode> GetOrderLineGeneratedBarcodes(OrderLine orderLine)
        {
            var orderLineGbc =  OrderLineGeneratedBCRepository.GetList(x => x.OrderLineId != null && x.OrderLineId.Value == orderLine.Id);
            return (orderLineGbc != null)? orderLineGbc.ToList() : null;
        }

        public virtual void SaveOrderLineBarCode(OrderLineGeneratedBarcode orderLineGbc)
        {
            if (orderLineGbc.Id == Guid.Empty)
                OrderLineGeneratedBCRepository.Add(orderLineGbc);
            else
                OrderLineGeneratedBCRepository.Update(orderLineGbc);

        }

        public virtual Order GetFullOrder(string orderId)
        {
            try
            {
                var order =
                    OrderRepository.GetSingle(
                        x => x.Id.ToString().Equals(orderId, StringComparison.CurrentCultureIgnoreCase));

                order.OrderLines =
                    OrderLineRepository.GetList(
                        x => x.OrderId.ToString().Equals(orderId, StringComparison.CurrentCultureIgnoreCase));

                order.Currency = CurrencyRepository.GetSingle(x => x.Id.Equals(order.CurrencyId));

                order.User = UserRepository.GetSingle(x => x.Id.Equals(order.UserId));

                return order;
            }
            catch (Exception ex)
            {
                Log("Error getting full order: GetFullOrder() Id:" + orderId + Environment.NewLine + ex.Message);
            }

            return null;
        }

        public virtual List<DiallingCode> GetAlldiallingDiallingCodes()
        {
            return DiallingCodeRepository.GetAll().ToList();
        }

        public virtual DiallingCode GetDiallingCode(string id)
        {
            return DiallingCodeRepository.GetSingle(x => x.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
        }

        public virtual bool OrderAllTicketShowMobile(Order order)
        {
            if (order.OrderLines == null)
                return false;

            return order.OrderLines.Select(orderLine => 
                TicketRepository.GetSingle(x => x.Id.Equals(orderLine.TicketId.Value)))
                .All(ticket => ticket == null || ticket.HasMobile);
        }

        public List<EcrOrderLineData> GetOrderLineDetails(string orderId)
        {
             var paramList = new List<SqlParameter>
            {
                new SqlParameter("orderid", orderId)
            };

            var orderLineData = QueryFunctions.DataTableFromStoredProcedure("SP_Order_GetOrderLineEcrVersions",
                paramList);

            var returnedItems = new List<EcrOrderLineData>();

            if (orderLineData == null || orderLineData.Rows == null || orderLineData.Rows.Count <= 0)
                return returnedItems;

            returnedItems.AddRange(from DataRow row in orderLineData.Rows
                select new EcrOrderLineData
                {
                    MicrositeId = row["Microsite_Id"].ToString(), 
                    MicrositeName = row["MicrositeName"].ToString(),
                    NewCheckoutEcrProductCode = (row["NCEcrProductCode"] != DBNull.Value)? 
                        row["NCEcrProductCode"].ToString() : string.Empty, 
                    NewCheckoutVersionId = (row["NewCKEcrVersionId"] != DBNull.Value) ? 
                        Convert.ToInt32(row["NewCKEcrVersionId"]) : 1, 
                    OrderLineId = row["Id"].ToString(),
                    UseQrCode = Convert.ToBoolean(row["UseQR"])
                });

            return returnedItems;
        }
    }
}
