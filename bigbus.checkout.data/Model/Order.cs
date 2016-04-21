
namespace bigbus.checkout.data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tb_Order")]
    public class Order
    {
        public Order()
        {
            OrderLines = new HashSet<OrderLine>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        [Column("User_Id")]
        public Guid? UserId { get; set; }

        [StringLength(500)]
        [Column("eMailAddress")]
        public string EmailAddress { get; set; }

        [Column("Currency_Id")]
        public Guid? CurrencyId { get; set; }

        public decimal? Total { get; set; }

        public bool? OpenForPrinting { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [StringLength(200)]
        public string UserName { get; set; }

        [StringLength(100)]
        public string LeadTravelerName { get; set; }

        [Column("Company_Id")]
        public Guid? CompanyId { get; set; }

        public decimal? AgentDiscount { get; set; }

        public int? TotalQuantity { get; set; }

        [StringLength(4)]
        [Column("CCLast4Digits")]
        public string CcLast4Digits { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderNumber { get; set; }

        [Required]
        [StringLength(3)]
        [Column("Language_Id")]
        public string LanguageId { get; set; }

        [StringLength(100)]
        public string GiftTravelerName { get; set; }

        public bool NotificationEmailSent { get; set; }

        public bool ShowGoogleCode { get; set; }

        [Column("NumbViewPDF")]
        public int? NumbViewPdf { get; set; }

        [Column("DatePDFLastViewed", TypeName = "datetime2")]
        public DateTime? DatePdfLastViewed { get; set; }

        [StringLength(100)]
        [Column("Centinel_OrderId")]
        public string CentinelOrderId { get; set; }

        [StringLength(100)]
        [Column("Centinel_TransactionId")]
        public string CentinelTransactionId { get; set; }

        [StringLength(100)]
        [Column("Centinel_Enrolled")]
        public string CentinelEnrolled { get; set; }

        [StringLength(100)]
        [Column("Centinel_ErrorNo")]
        public string CentinelErrorNo { get; set; }

        [StringLength(1000)]
        [Column("Centinel_ErrorDesc")]
        public string CentinelErrorDesc { get; set; }

        [StringLength(100)]
        [Column("Centinel_ECI")]
        public string CentinelEci { get; set; }

        [StringLength(1000)]
        [Column("Centinel_ACSURL")]
        public string CentinelAcsurl { get; set; }

        [StringLength(1000)]
        [Column("Centinel_PAYLOAD")]
        public string CentinelPayload { get; set; }

        [StringLength(100)]
        [Column("Centinel_PIType")]
        public string CentinelPiType { get; set; }

        [StringLength(100)]
        [Column("Centinel_TransactionType")]
        public string CentinelTransactionType { get; set; }

        [StringLength(100)]
        [Column("Centinel_PAResStatus")]
        public string CentinelPaResStatus { get; set; }

        [StringLength(100)]
        [Column("Centinel_SignatureVerification")]
        public string CentinelSignatureVerification { get; set; }

        [StringLength(1000)]
        [Column("Centinel_TermURL")]
        public string CentinelTermUrl { get; set; }

        [StringLength(100)]
        [Column("Centinel_XID")]
        public string CentinelXid { get; set; }

        [StringLength(100)]
        [Column("Centinel_CAVV")]
        public string CentinelCavv { get; set; }

        [StringLength(50)]
        [Column("Centinel_DIG")]
        public string CentinelDig { get; set; }

        [StringLength(1000)]
        public string Message { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateModified { get; set; }

        [StringLength(50)]
        public string AgentRef { get; set; }

        [StringLength(50)]
        public string ClientIp { get; set; }

        [Column("PaymentTransaction_Id")]
        public Guid? PaymentTransactionId { get; set; }

        [StringLength(11)]
        public string AuthCodeNumber { get; set; }

        public bool OrderConfirmationViewed { get; set; }

        public bool SentQrCodeToMobile { get; set; }

        public string QrCodeUniqueHash { get; set; }

        public bool IsMobileAppOrder { get; set; }

        [Column("Basket_Id")]
        public Guid? BasketId { get; set; }

        [Column("Session_Id")]
        public Guid? SessionId { get; set; }

        [StringLength(50)]
        public string WorldPayMerchantId { get; set; }

        [StringLength(50)]
        public string GatewayReference { get; set; }

        [StringLength(150)]
        public string NameOnCard { get; set; }

        [StringLength(150)]
        [Column("PayPal_Token")]
        public string PayPalToken { get; set; }

        [StringLength(150)]
        [Column("PayPal_Order_Id")]
        public string PayPalOrderId { get; set; }

        [StringLength(150)]
        [Column("PayPal_Payer_Id")]
        public string PayPalPayerId { get; set; }

        [StringLength(1500)]
        public string EcrBookingBarCode { get; set; }

        [StringLength(1500)]
        public string EcrBookingShortReference { get; set; }

        [StringLength(1500)]
        public string EcrSupplierConfirmationNumber { get; set; }

        [StringLength(500)]
        public string ExternalBarcode { get; set; }

        [StringLength(100)]
        public string ExternalOrderId { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }

        public virtual User User { get; set; }

        public virtual Currency Currency { get; set; }
       
    }
}
