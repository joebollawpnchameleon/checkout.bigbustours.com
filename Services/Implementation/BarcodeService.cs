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

            var dataSet = BarcodeDBFunctions.DataSetFromStoredProcedure("sp_Barcode_GetNextBarcode", paramList);
            var barcodePrefix = dataSet.Tables[0].Rows[0]["BarcodePrefix"].ToString();
            var padLength = 12 - barcodePrefix.Length;
            var nextAvailableNumber = dataSet.Tables[0].Rows[0]["NextAvailableBarcode"].ToString().PadLeft(padLength, '0');

            return string.Concat(barcodePrefix, nextAvailableNumber);
        }

        public List<OrderLineGeneratedBarcode> GetOrderLineGeneratedBarcodes(string orderLineId)
        {
            var orderLinesGBarcodes =
                OrderLineGeneratedBCRepository.GetList(
                    x => x.OrderLineId != null &&
                        x.OrderLineId.Value.ToString().Equals(orderLineId, StringComparison.CurrentCultureIgnoreCase));

            return orderLinesGBarcodes != null ? orderLinesGBarcodes.ToList() : null;
        } 
    }
}
