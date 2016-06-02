using bigbus.checkout.data.Model;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class BarcodeService : BaseService, IBarcodeService
    {
        public virtual string GetNextBarcode(Ticket ticket, string orderLineTicketType)
        {           
            var paramList = new List<SqlParameter> {
                new SqlParameter("MicroSite_Id", ticket.MicroSiteId),
                new SqlParameter("Ticket_Type", orderLineTicketType),
                new SqlParameter("Ticket_Id", ticket.Id),
                new SqlParameter("Ticket_TicketType", ticket.TicketType),
                new SqlParameter("IsInDrEnvironment", false)//*** check the dr environment
            };

            var dataSet = BarcodeDBFunctions.DataSetFromStoredProcedure("GetNextBarcode", paramList);

            
            string BarcodePrefix = dataSet.Tables[0].Rows[0]["BarcodePrefix"].ToString();
            int PadLength = 12 - BarcodePrefix.Length;
            string NextAvailableNumber = dataSet.Tables[0].Rows[0]["NextAvailableBarcode"].ToString().PadLeft(PadLength, '0');

            return string.Concat(BarcodePrefix, NextAvailableNumber);
        }
    }
}
