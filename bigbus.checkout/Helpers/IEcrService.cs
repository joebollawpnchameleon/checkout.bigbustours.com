using bigbus.checkout.EcrWServiceRefV3;
using Common.Model.Interfaces;
using System.Collections.Generic;

namespace bigbus.checkout.Helpers
{
    public interface IEcrService
    {
        Product[] GetProductList();

        Product[] GetProductList(ICacheProvider cacheProvider);

        AvailabilityResponse GetAvailability(List<AvailabilityTransactionDetail> availabilityTransactionDetails);

        BookingResponse SubmitBooking(int ordernumber, AvailabilityResponse availability, List<BookingTransactionDetail> bookingDetails);
    }
}
