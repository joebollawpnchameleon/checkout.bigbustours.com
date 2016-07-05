
namespace Common.Model
{
    public class TestTicket
    {
        public string TicketId { get; set; }

        public string TicketType { get; set; }

        public string TicketName { get; set; }

        public string MicroSiteId { get; set; }

        public int MicroSiteEcrVersionId { get; set; }

        public string EcrProductCode { get; set; }

        public string CurrencyCode { get; set; }

        public string CurrencySymbol { get; set; }

        public string QuantityOptionHtml { get; set; }

        public string TotalOptionHtml { get; set; }

        public string AdditionalDetailsCsv { get; set; }

        public string PriceOptionHtml { get; set; }
        
    }
}
