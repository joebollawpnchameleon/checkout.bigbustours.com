using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace bigbus.checkout.data.Model
{   

    [Table("tb_OrderLine_GeneratedBarcode")]
    public class OrderLineGeneratedBarcode
    {
        public Guid Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        [Column("OrderLine_Id")]
        public Guid? OrderLineId { get; set; }

        [StringLength(50)]
        public string GeneratedBarcode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RedeemedDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RefundDate { get; set; }

        [StringLength(50)]
        public string RefundReason { get; set; }

        [StringLength(256)]
        public string Comments { get; set; }
    }
}
