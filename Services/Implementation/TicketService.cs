using System;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Services.Infrastructure;

namespace Services.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly IGenericDataRepository<Ticket> _ticketRepository;
 
        public TicketService(IGenericDataRepository<Ticket> ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public Ticket GetTicketBySku(string sku)
        {
            return
                _ticketRepository.GetSingle(x => !string.IsNullOrEmpty(x.EcrProductCode) 
                    && x.EcrProductCode.Trim().Equals(sku.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        public Ticket GetTicketById(string id)
        {
            return
                _ticketRepository.GetSingle(x => x.Id.ToString().Equals(id.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
