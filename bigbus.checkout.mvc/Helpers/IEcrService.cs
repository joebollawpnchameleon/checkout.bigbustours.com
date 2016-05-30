using bigbus.checkout.mvc.EcrApi3ServiceRef;
using System.Collections.Generic;

namespace bigbus.checkout.mvc.Helpers
{
    public interface IEcrService
    {
        Product[] GetProductList();

        AvailabilityResponse GetAvailability(List<AvailabilityTransactionDetail> availabilityTransactionDetails);

        BookingResponse SubmitBooking(int ordernumber, AvailabilityResponse availability, List<BookingTransactionDetail> bookingDetails);

    }
}