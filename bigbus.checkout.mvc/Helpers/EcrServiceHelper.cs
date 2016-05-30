using System;
using System.Collections.Generic;
using System.Linq;
using bigbus.checkout.data.Model;
using bigbus.checkout.mvc.EcrApi3ServiceRef;

namespace bigbus.checkout.Helpers
{
    public class EcrServiceHelper
    {
         public static List<AvailabilityTransactionDetail> GetAvailabilityFromOrderLines(List<OrderLine> orderLines)
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

        public static List<BookingTransactionDetail> GetBookingTransactionDetails(List<OrderLine> orderLines, string currencyCode)
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

    }
}