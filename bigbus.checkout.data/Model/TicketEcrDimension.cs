using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bigbus.checkout.data.Model
{
    [Table("tb_Ticket_EcrProduct_Dimension")]
    public class TicketEcrDimension
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Amount { get; set; }

        public string CurrencyCode { get; set; }

        public string CurrencyId { get; set; }

        public string ProductDimensionUID { get; set; }

        public string TicketId { get; set; }

        public string EcrSysId { get; set; }
    }
}
