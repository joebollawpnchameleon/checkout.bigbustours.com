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
            var availTransactionDetails = new List<AvailabilityTransactionDetail>
            {
                new AvailabilityTransactionDetail
                {
                     ProductDimensionUID = "c4bf36f4-acf4-4da8-b8ca-f4f9ce9345a0",
                     QTY = 2,
                     ValidFrom = DateTime.Now,
                },
                new AvailabilityTransactionDetail
                {
                     ProductDimensionUID = "E5CD49D7-3575-464F-AE3F-A093C83261BB",
                     QTY = 3,
                     ValidFrom = DateTime.Now.AddDays(2)
                }
            };

            return availTransactionDetails;
        }

        public static List<BookingTransactionDetail> GetBookingTransactionDetails(List<OrderLine> orderLines)
        {
            var transactionDetails = new List<BookingTransactionDetail>
                {
                    new BookingTransactionDetail
                    {
                        CurrencyCode = "GBP", DiscountCode = "Coupon 1", Notes = "Note",
                        Price =67.45M, ProductDimensionUID = "c4bf36f4-acf4-4da8-b8ca-f4f9ce9345a0",
                    },
                    new BookingTransactionDetail
                    {
                        CurrencyCode = "GBP", DiscountCode = "Coupon 2", Notes = "Note2",
                        Price =37.45M, ProductDimensionUID = "E5CD49D7-3575-464F-AE3F-A093C83261BB",
                    }
                };

            return transactionDetails;
        }

    }
}