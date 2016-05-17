using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using bigbus.checkout.data.Model;
using bigbus.checkout.EcrServiceRef;
using Services.Infrastructure;
using bigbus.checkout.EcrWServiceRefV3;

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
                     QTY = quantity, ValidFrom = DateTime.Now, ProductDimensionUID = orderLine.EcrProductDimensionId
                 }).ToList();
         }

        public static List<BookingTransactionDetail> GetBookingTransactionDetails(List<OrderLine> orderLines, string currencyCode)
        {
            return (from orderLine in orderLines
                let netLineValue = orderLine.NettOrderLineValue ?? (decimal) 0.0
                select new BookingTransactionDetail
                {
                    CurrencyCode = currencyCode, Notes = "Note", Price = netLineValue, ProductDimensionUID = orderLine.EcrProductDimensionId
                }).ToList();
        }
    }
}