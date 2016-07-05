using System;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Common.Enums;
using Services.Infrastructure;
using Common.Model;
using System.Collections.Generic;
using System.Data;

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

        public virtual void CreateTicket(Ticket ticket)
        {
            try
            {
                _ticketRepository.Add(ticket);
            }
            catch (Exception ex)
            {
                Log("TicketService => CreateTicket() sysid" + ticket.NcEcrProductCode + ex.Message);
            }
        }

        public virtual void CreateTicketEcrDimension(TicketEcrDimension dimension)
        {
            EcrProductDimensionRepository.Add(dimension);
        }

        public virtual List<TestTicket> GetTestTickets()
        {
            var dataTable = QueryFunctions.DataTableFromStoredProcedure("GetEcrTicketList");

            if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count < 1)
                return null;

            var ticketList = new List<TestTicket>();

            foreach(DataRow row in dataTable.Rows)
            {
                ticketList.Add(
                    new TestTicket
                    {
                        TicketId = row["Id"].ToString(),
                        TicketName = row["Name"].ToString(),
                        TicketType = row["TicketType"].ToString(),
                        MicroSiteId = row["Microsite_Id"].ToString(),
                        CurrencyCode = row["CurrencyCode"].ToString(),
                        CurrencySymbol = row["CurrencySymbol"].ToString(),
                        EcrProductCode = row["NCEcrProductCode"].ToString(),
                        AdditionalDetailsCsv = row["DetailsCsv"].ToString(),
                        MicroSiteEcrVersionId = Convert.ToInt32(row["MSEcrVersionid"]),
                    }
                );
            }

            return ticketList;
        }
    }
}
