using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using bigbus.checkout.data.Model;
using bigbus.checkout.Ecr1ServiceRef;
using bigbus.checkout.EcrServiceRef;
using Services.Infrastructure;
using bigbus.checkout.EcrWServiceRefV3;
using Common.Enums;
using Common.Model;
using Services.Implementation;

namespace bigbus.checkout.Helpers
{
    public class EcrApi3ServiceHelper : IEcrApi3ServiceHelper
    {
        private readonly IEcrService _ecrService;
        private readonly ICheckoutService _checkoutService;
        private readonly StringBuilder _bookingErrors;

        public string GetLastBookingErrors()
        {
            return _bookingErrors.ToString();
        }

        public EcrApi3ServiceHelper(IEcrService ecrService, ICheckoutService checkoutService)
        {
            _ecrService = ecrService;
            _checkoutService = checkoutService;
            _bookingErrors = new StringBuilder();
        }

        public List<AvailabilityTransactionDetail> GetAvailabilityFromOrderLines(List<OrderLine> orderLines)
         {
             return (from orderLine in orderLines
                     let quantity = orderLine.TicketQuantity ?? 1
                     select new AvailabilityTransactionDetail
                     {
                         QTY = quantity,
                         ValidFrom = DateTime.Now,
                         ProductDimensionUID = orderLine.EcrProductDimensionId
                     }).ToList();
         }

        public List<BookingTransactionDetail> GetBookingTransactionDetails(List<OrderLine> orderLines, string currencyCode)
        {
            return (from orderLine in orderLines
                    let netLineValue = orderLine.NettOrderLineValue ?? (decimal)0.0
                    select new BookingTransactionDetail
                    {
                        CurrencyCode = currencyCode,
                        Notes = "Note",
                        Price = netLineValue,
                        ProductDimensionUID = orderLine.EcrProductDimensionId
                    }).ToList();
        }


        public EcrWServiceRefV3.BookingResponse SendBookingToEcr3(Order order, List<OrderLine> orderLineData)
        {
            _bookingErrors.Clear();

            var availability = GetAvailabilityFromOrderLines(orderLineData);

            var availabilityResponse = _ecrService.GetAvailability(availability);

            if (availabilityResponse == null)
            {
                _bookingErrors.AppendLine("Availability failed for OrderId: " + order.Id);
                return null;
            }

            if (string.IsNullOrEmpty(availabilityResponse.TransactionReference))
            {
                _bookingErrors.AppendLine("Availability response returned no transaction ref OrderId: " + order.Id +
                    System.Environment.NewLine + " error: " + availabilityResponse.ErrorDescription);
                return null;
            }

            var bookingTransactions = GetBookingTransactionDetails(orderLineData, order.Currency.ISOCode);

            var response = _ecrService.SubmitBooking(order.OrderNumber, availabilityResponse, bookingTransactions);

            if (response != null && response.Status == (int)EcrResponseStatus.Success) return response;

            _bookingErrors.AppendLine("Send to Ecr Failed with error " + (response == null ? "Booking process failed " : response.ErrorDescription));
            return null;
        }

        private string CreateRandomNumber()
        {
            var rnd = new Random();
            var num = string.Empty;

            lock (rnd)
            {
                Thread.Sleep(20);
                for (var i = 0; i < 10; i++)
                {
                    num += rnd.Next(0, 9);
                }
                num = num.Substring(0, 10);
            }

            return num;
        }

        /*** handle attractions in this case separately from other tickets */
        public bool SendBookingToEcr1(Order order, List<OrderLine> selectedOrderLines,List<EcrOrderLineData> orderlineData)
        {
            _bookingErrors.Clear();

            bool result;
            var productCode = string.Empty;
            var bDoEcr = false;
            var ticketDate = DateUtil.NullDate;

            _bookingErrors.AppendLine("Send to Ecr1 started. SendBookingToEcr1() - OrderId:" + order.Id);

            if (string.IsNullOrWhiteSpace(order.AuthCodeNumber))
            {
                order.AuthCodeNumber = CreateRandomNumber();
            }

            foreach (var orderLine in selectedOrderLines)
            {
                var ecrData = orderlineData.FirstOrDefault(x => x.OrderLineId.Equals(orderLine.Id.ToString(), StringComparison.CurrentCultureIgnoreCase));

                if (ecrData == null || !ecrData.UseQrCode)
                    continue;
                
                bDoEcr = true;
                var barcodes = _checkoutService.GetOrderLineGeneratedBarcodes(orderLine);

                foreach (var orderLineGeneratedBarcode in barcodes)
                {
                    productCode += orderLineGeneratedBarcode.GeneratedBarcode + "01";

                    var lineprice = Convert.ToInt32(orderLine.TicketCost * 100);

                    productCode += lineprice.ToString("000000"); //The "lineprice" string needs to be at least 6 characters long
                }

                if (ticketDate != DateUtil.NullDate &&
                    orderLine.TicketDate != DateUtil.NullDate &&
                    orderLine.TicketDate != DateTime.MinValue)
                {
                    if (orderLine.TicketDate != null) ticketDate = orderLine.TicketDate.Value;
                }
            }

            if (!bDoEcr)
            {
                //("Order lines for orderid " + order.Id + " not using QR");
                return true;
            }

            var orderTotal = Convert.ToInt32(order.Total * 100);
            var sixDigitOrderTotalString = orderTotal.ToString("000000"); // Again ensure that the string is at least 6 characters long
            var tenDigitOrderNumber = order.OrderNumber.ToString("0000000000"); // This time we need to ensure the string is at least 10 characters long
            var qrCurrencyCode = order.Currency.QrId.ToString("00"); // Ensure it's at least two characters long

            var qrCodeDataString =
                tenDigitOrderNumber +
                order.AuthCodeNumber +
                qrCurrencyCode +
                sixDigitOrderTotalString +
                ticketDate.ToString("ddMMyyyy") +
                productCode;

            //var uptodateOrder = GetObjectFactory().GetById<Order>(theOrder.Id);
            var dtsClient = new DtsClient();

            try
            {
                var authcodeForEcr = order.AuthCodeNumber[4] + order.AuthCodeNumber[1] + order.AuthCodeNumber[8] + order.AuthCodeNumber[5];

                if (ConfigurationManager.AppSettings["EcrApiV1UseMethodVersion"].Equals("1"))
                {
                    var agentref = string.IsNullOrWhiteSpace(order.AgentRef)
                        ? "BigBusToursDotComWebSales"
                        : order.AgentRef;
                    _bookingErrors.AppendLine("ECR version 2 - putex with agent ref" + agentref + "Ordernumber" + order.OrderNumber);
                    result =
                        dtsClient.PutEx(
                            order.OrderNumber,
                            Convert.ToInt32(authcodeForEcr),
                            productCode,
                            orderTotal,
                            order.DateCreated,
                            ticketDate,
                            string.IsNullOrWhiteSpace(order.AgentRef)
                                ? "BigBusToursDotComWebSales"
                                : order.AgentRef);

                }
                else
                {
                    _bookingErrors.AppendLine("ECR version 1 -old version" + order.OrderNumber);

                    result =
                        dtsClient.Put(
                            order.OrderNumber,
                            Convert.ToInt32(authcodeForEcr),
                            productCode,
                            orderTotal,
                            order.DateCreated,
                            ticketDate);
                }

                order.CentinelEci = result.ToString();
            }
            catch (Exception exception)
            {
                _bookingErrors.AppendLine("Send to Ecr V1 Error: " + exception.Message);
                return false;
            }

            order.CentinelAcsurl = qrCodeDataString;
            _checkoutService.SaveOrder(order);

            return result;
        }
    }
}