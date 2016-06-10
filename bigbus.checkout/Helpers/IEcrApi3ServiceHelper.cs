using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bigbus.checkout.data.Model;
using Common.Model;

namespace bigbus.checkout.Helpers
{
    public interface IEcrApi3ServiceHelper
    {
        string GetLastBookingErrors();

        EcrWServiceRefV3.BookingResponse SendBookingToEcr3(Order order, List<OrderLine> orderLineData);

        bool SendBookingToEcr1(Order order, List<OrderLine> selectedOrderLines, List<EcrOrderLineData> orderlineData);
    }
}
