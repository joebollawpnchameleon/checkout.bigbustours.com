using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace bigbus.checkout.data.Model
{
  

    [Table("tb_Session")]
    public class Session
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("PaymentTransaction_Id")]
        public Guid? PaymentTransactionId { get; set; }

        [Column("Basket_Id")]
        public Guid? BasketId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        [Column("User_Id")]
        public Guid? UserId { get; set; }

        public Guid? AgentManagerId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateToExpire { get; set; }

        [StringLength(50)]
        [Column("Currency_Id")]
        public string CurrencyId { get; set; }

        [StringLength(100)]
        [Column("PayPal_Order_Id")]
        public string PayPalOrderId { get; set; }

        [StringLength(100)]
        [Column("PayPal_Token")]
        public string PayPalToken { get; set; }

        [StringLength(100)]
        [Column("PayPal_Payer_Id")]
        public string PayPalPayerId { get; set; }

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

        [Column("Centinel_ErrorDesc", TypeName = "ntext")]
        public string CentinelErrorDesc { get; set; }

        [StringLength(100)]
        [Column("Centinel_ECI")]
        public string CentinelEci { get; set; }

        [StringLength(500)]
        [Column("Centinel_ACSURL")]
        public string CentinelAcsurl { get; set; }

        [Column("Centinel_PAYLOAD", TypeName = "ntext")]
        public string CentinelPayload { get; set; }

        [StringLength(100)]
        [Column("Centinel_PIType")]
        public string CentinelPiType { get; set; }

        [StringLength(100)]
        [Column("Centinel_TransactionType")]
        public string CentinelTransactionType { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateModified { get; set; }

        [Column(TypeName = "ntext")]
        public string Message { get; set; }

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

        public bool IsMobileAppSession { get; set; }

        public bool InCheckoutProccess { get; set; }

        public bool InOrderCreationProcess { get; set; }

        public bool AgentUseCustomersAddress { get; set; }

        [Column("AgentFakeUser_Id")]
        public Guid? AgentFakeUserId { get; set; }

        public bool AgentIsTradeTicketSale { get; set; }

        [StringLength(50)]
        public string AgentNameToPrintOnTicket { get; set; }

        public bool InPaypalCheckoutProcess { get; set; }

        public bool BookingTimesNeedsConfirming { get; set; }
    }
}
