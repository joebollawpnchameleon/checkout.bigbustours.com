
namespace bigbus.checkout.data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("tb_MicroSite")]
    public class MicroSite
    {
        public MicroSite()
        {
            MicroSiteLanguage = new HashSet<MicroSiteLanguage>();
        }

        [StringLength(50)]
        public string Id { get; set; }

        [StringLength(3)]
        [Column("Language_Id")]
        public string LanguageId { get; set; }

        [StringLength(50)]
        [Column("Currency_Id")]
        public string CurrencyId { get; set; }

        [Column("RedTour_Id")]
        public Guid? RedTourId { get; set; }

        [Column("BlueTour_Id")]
        public Guid? BlueTourId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [Column("HomeOfferImage_Id")]
        public Guid? HomeOfferImageId { get; set; }

        [StringLength(15)]
        public string Mode { get; set; }

        public bool MultilingualActive { get; set; }

        public int? DisplayOrder { get; set; }

        [Column("Offer_Id")]
        public int OfferId { get; set; }

        [StringLength(150)]
        public string DotNetTimeZone { get; set; }

        [StringLength(150)]
        public string DotNetTimeZoneStandardName { get; set; }

        [StringLength(300)]
        public string ContactDetails { get; set; }

        [StringLength(300)]
        public string ContactNumber { get; set; }

        public bool CollectTickets { get; set; }

        public string CollectTicketsDetails { get; set; }

        public bool IsUS { get; set; }

        public bool UseQR { get; set; }

        [StringLength(500)]
        public string RefundEmail { get; set; }

        public bool ShowTrustPilotCode { get; set; }

        [StringLength(150)]
        public string TrustPilotUrl { get; set; }

        [StringLength(10)]
        public string AffiliateWindowMerchantId { get; set; }

        [StringLength(50)]
        public string AffiliateWindowTourCommissionLabel { get; set; }

        [StringLength(50)]
        public string AffiliateWindowAttractionCommissionLabel { get; set; }

        public int? ViatorWidgetDestinationId { get; set; }

        [StringLength(50)]
        public string GoogleRemarketingConversionId { get; set; }

        [StringLength(50)]
        public string GoogleRemarketingConversionLabel { get; set; }

        public bool ECRVersion2Enabled { get; set; }

        public bool IsActiveAgentTandC { get; set; }

        public int EcrVersionId { get; set; }

        public int NewCKEcrVersionId { get; set; }

        public virtual ICollection<MicroSiteLanguage> MicroSiteLanguage { get; set; }
        
    }
}
