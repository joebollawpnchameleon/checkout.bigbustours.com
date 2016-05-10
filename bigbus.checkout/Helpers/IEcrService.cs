using bigbus.checkout.EcrWServiceRefV3;
using System.Collections.Generic;

namespace bigbus.checkout.Helpers
{
    public interface IEcrService
    {
        Product[] GetProductList();

        AvailabilityResponse GetAvailability(List<AvailabilityTransactionDetail> availabilityTransactionDetails);

        BookingResponse SubmitBooking(int ordernumber, AvailabilityResponse availability, List<BookingTransactionDetail> bookingDetails);
    }
}
