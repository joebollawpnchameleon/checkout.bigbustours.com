using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace bigbus.checkout.data.Model
{
    [Table("tb_Ecr_Barcodes")]
    public class EcrOrderLineBarcode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
               
        public DateTime DateCreated { get; set; }
       
        public string TicketId { get; set; }
        
        public int OrderNumber { get; set; }

        public string OrderLineId { get; set; }

        public Guid ImageId { get; set; }
    }
}
