using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bigbus.checkout.data.Model
{
    [Table("tb_Basket")]
    public class Basket
    {
        public Basket()
        {
            BasketLines = new HashSet<BasketLine>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        [Column("User_Id")]
        public Guid? UserId { get; set; }

        [Column("Currency_Id")]
        public Guid? CurrencyId { get; set; }

        public decimal Total { get; set; }

        [StringLength(100)]
        public string LeadTravelerName { get; set; }

        [StringLength(100)]
        public string GiftTravelerName { get; set; }

        [StringLength(500)]
        public string ExternalBarcode { get; set; }

        [StringLength(100)]
        public string ExternalOrderId { get; set; }

        public decimal? DiscountValue { get; set; }

        [StringLength(100)]
        public string ExternalCookieValue { get; set; }

        [StringLength(100)]
        public string ExternalCoupon { get; set; }

        public string PurchaseLanguage { get; set; }

        public virtual ICollection<BasketLine> BasketLines { get; set; }
    }
}
