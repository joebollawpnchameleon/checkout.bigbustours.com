
namespace bigbus.checkout.data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tb_Orderline")]
    public class OrderLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("Order_Id")]
        public Guid? OrderId { get; set; }

        [Column("Ticket_Id")]
        public Guid? TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }

        public bool? FixedDateTicket { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TicketDate { get; set; }

        [StringLength(50)]
        public string TicketType { get; set; }

        public int? TicketQuantity { get; set; }

        public decimal? TicketCost { get; set; }

        [StringLength(20)]
        [Column("Promotion_Id")]
        public string PromotionId { get; set; }

        [StringLength(10)]
        [Column("Promotion_Value")]
        public string PromotionValue { get; set; }

        public decimal? GrossOrderLineValue { get; set; }

        public decimal? NettOrderLineValue { get; set; }

        [StringLength(10)]
        public string DepartureTimeHour { get; set; }

        [StringLength(10)]
        public string DepartureTimeMinute { get; set; }

        [StringLength(100)]
        public string DeparturePoint { get; set; }

        [StringLength(100)]
        [Column("Microsite_Id")]
        public string MicrositeId { get; set; }

        [StringLength(50)]
        [Column("TicketTOrA")]
        public string TicketTorA { get; set; }

        [StringLength(50)]
        public string AttractionName { get; set; }

        [StringLength(50)]
        public string GeneratedBarcode { get; set; }

        public bool? IsPromotionTicket { get; set; }

        [StringLength(50)]
        public string PromotionType { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TicketRedeemedDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TicketAttractionDate { get; set; }

        [StringLength(10)]
        public string TicketAttractionTimeHour { get; set; }

        [StringLength(10)]
        public string TicketAttractionTimeMin { get; set; }

        [StringLength(10)]
        public string AttractionTimeSection { get; set; }

        [StringLength(100)]
        public string EcrProductDimensionId { get; set; }
        
        public virtual Order Order { get; set; }
        
        public virtual MicroSite MicroSite { get; set; }

        public bool IsTour
        {
            get { return TicketTorA.Equals("Tour", StringComparison.InvariantCultureIgnoreCase); }
        }

        public bool IsAttraction
        {
            get { return TicketTorA.Equals("Attraction", StringComparison.InvariantCultureIgnoreCase); }
        }
    }
}
