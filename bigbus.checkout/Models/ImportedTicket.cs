using System;
using System.Collections.Generic;


namespace bigbus.checkout.Models
{
    public class ImportedTicket
    {
        public string EcrProductUid { get; set; }

        public string EcrProductSku { get; set; }
 
        public DateTime StartDate { get; set; }
        
        public string TicketType { get; set; }

        public string MicroSiteId { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public string FulfilmentInstructions { get; set; }

        public List<EcrProductDimension> EcrProductDimensionList { get; set; }
    }

    public class EcrProductDimension
    {
        public string UiDimensionId { get; set; }

        public string DimensionSku { get; set; }

        public string EcrProductSku { get; set; }

        public string AgeBand { get; set; }
    }
}