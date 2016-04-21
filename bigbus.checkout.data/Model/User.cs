using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bigbus.checkout.data.Model
{
    [Table("tb_User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(500)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Password { get; set; }

        public string PasswordHash { get; set; }

        public int PasswordVersion { get; set; }

        [StringLength(100)]
        public string ReminderChallenge { get; set; }

        [StringLength(100)]
        public string ReminderAnswer { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(100)]
        public string Firstname { get; set; }

        [StringLength(100)]
        public string Lastname { get; set; }

        [StringLength(255)]
        public string AddressLine1 { get; set; }

        [StringLength(255)]
        public string AddressLine2 { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(15)]
        public string PostCode { get; set; }

        [StringLength(100)]
        public string StateProvince { get; set; }

        [StringLength(100)]
        [Column("Country_Id")]
        public string CountryId { get; set; }

        public bool? ReceiveNewsletter { get; set; }

        [StringLength(3)]
        [Column("Language_Id")]
        public string LanguageId { get; set; }

        [Column("Currency_Id")]
        public Guid? CurrencyId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateRegistered { get; set; }

        [StringLength(50)]
        public string JobTitle { get; set; }

        [Column("Basket_Id")]
        public Guid? BasketId { get; set; }

        [Column("Company_Id")]
        public Guid? CompanyId { get; set; }

        public bool Authorised { get; set; }

        [StringLength(50)]
        [Column("AuthorisedBy_Id")]
        public string AuthorisedById { get; set; }

        public bool IsAgent { get; set; }

        [StringLength(50)]
        [Column("AgentProfile_Id")]
        public string AgentProfileId { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string FaxNumber { get; set; }

        [StringLength(150)]
        [Column("WebsiteURL")]
        public string WebsiteUrl { get; set; }

        [StringLength(50)]
        public string CompanyName { get; set; }

        [StringLength(4000)]
        public string AgentNote { get; set; }

        public bool CanViewCompOrders { get; set; }

        [StringLength(50)]
        [Column("MicroSite_Id")]
        public string MicroSiteId { get; set; }

        [StringLength(50)]
        public string CityPrefer { get; set; }

        [StringLength(400)]
        public string FriendlyEmail { get; set; }

        public bool W9OnFile { get; set; }

        public bool MobileTicketAgent { get; set; }

        [Column("IsECR")]
        public bool IsEcr { get; set; }

        public bool AllowCashSales { get; set; }

        public bool AllowCreditCardSales { get; set; }

        public bool IsAgentTandCAccepted { get; set; }
       
    }
}
