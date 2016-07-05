using bigbus.checkout.data.Model;
using Common.Model;
using System.Collections.Generic;

namespace Services.Infrastructure
{
    public interface ITicketService
    {
        Ticket GetTicketBySku(string sku, int ecrVersionId);

        Ticket GetTicketByEcrSysId(string ecrSysId);

        Ticket GetTicketById(string id);

        Ticket GetTicketByProductDimensionUid(string prodDimentionUid);

        void CreateTicket(Ticket ticket);

        void CreateTicketEcrDimension(TicketEcrDimension dimension);

        List<TestTicket> GetTestTickets();
    }
}
