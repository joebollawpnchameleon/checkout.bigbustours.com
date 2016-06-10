using bigbus.checkout.data.Model;

namespace Services.Infrastructure
{
    public interface ITicketService
    {
        Ticket GetTicketBySku(string sku, int ecrVersionId);

        Ticket GetTicketByEcrSysId(string ecrSysId);

        Ticket GetTicketById(string id);

        Ticket GetTicketByProductDimensionUid(string prodDimentionUid);
    }
}
