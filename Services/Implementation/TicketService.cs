using System;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Common.Enums;
using Services.Infrastructure;

namespace Services.Implementation
{
    public class TicketService : BaseService, ITicketService
    {
        private readonly IGenericDataRepository<Ticket> _ticketRepository;
 
        public TicketService(IGenericDataRepository<Ticket> ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public virtual Ticket GetTicketBySku(string sku, int ecrVersionId)
        {
            switch (ecrVersionId)
            {
                case (int)EcrVersion.Two:
                    return
                        _ticketRepository.GetSingle(x => !string.IsNullOrEmpty(x.EcrProductCode)
                            && x.EcrProductCode.Trim().Equals(sku.Trim(), StringComparison.CurrentCultureIgnoreCase));
                default: //by default we use NcEcrProductcode
                    return
                        _ticketRepository.GetSingle(x => !string.IsNullOrEmpty(x.NcEcrProductCode)
                            && x.NcEcrProductCode.Trim().Equals(sku.Trim(), StringComparison.CurrentCultureIgnoreCase));
            }
            
        }

        public virtual Ticket GetTicketByEcrSysId(string ecrSysId)
        {
            return
                _ticketRepository.GetSingle(x => !string.IsNullOrEmpty(x.NcEcrProductCode)
                    && x.NcEcrProductCode.Trim().Equals(ecrSysId.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        public virtual Ticket GetTicketByProductDimensionUid(string prodDimentionUid)
        {
            var ecrTicketDimension = EcrProductDimensionRepository.GetSingle(x => x.Id.ToString().Equals(prodDimentionUid, StringComparison.CurrentCultureIgnoreCase));

            if (ecrTicketDimension == null)
                return null;
            
            return GetTicketById(ecrTicketDimension.TicketId);
        }

        public virtual Ticket GetTicketById(string id)
        {
            return
                _ticketRepository.GetSingle(x => x.Id.ToString().Equals(id.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
