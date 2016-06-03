using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bigbus.checkout.data.Model
{
    [Table("tb_BasketLine")]
    public class BasketLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Basket Basket { get; set; }

        [Column("Basket_Id")]
        public Guid? BasketId { get; set; }
        
        [Column("Ticket_Id")]
        public Guid? TicketId { get; set; }

        public bool? FixedDateTicket { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TicketDate { get; set; }

        [StringLength(50)]
        public string TicketType { get; set; }

        public Ticket Ticket { get; set; }

        public int? TicketQuantity { get; set; }

        [StringLength(20)]
        [Column("Promotion_Id")]
        public string PromotionId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateAdded { get; set; }

        [StringLength(10)]
        public string DepartureTimeHour { get; set; }

        [StringLength(10)]
        public string DepartureTimeMinute { get; set; }

        [StringLength(100)]
        public string DeparturePoint { get; set; }

        public bool IsPromotionTicket { get; set; }

        public bool PromotionTicketRemoved { get; set; }

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

        public decimal? Price { get; set; }

        public decimal? Discount { get; set; }

        public decimal? LineTotal { get; set; }
    }
}
