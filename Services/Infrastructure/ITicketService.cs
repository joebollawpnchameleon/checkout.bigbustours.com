using bigbus.checkout.data.Model;

namespace Services.Infrastructure
{
    public interface ITicketService
    {
        Ticket GetTicketBySku(string sku);

        Ticket GetTicketById(string id);
    }
}
