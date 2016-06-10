using bigbus.checkout.data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    public interface IBarcodeService
    {
        string GetNextBarcode(Ticket ticket, string orderLineTicketType);

        List<OrderLineGeneratedBarcode> GetOrderLineGeneratedBarcodes(string orderLineId);
    }
}
