

namespace bigbus.checkout.data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tb_Language")]
    public class Language
    {

        [StringLength(3)]
        public string Id { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }

        [StringLength(3)]
        [Column("PreferedFallback_Id")]
        public string PreferedFallbackId { get; set; }

        [StringLength(10)]
        [Column("shortcode")]
        public string ShortCode { get; set; }

        [StringLength(500)]
        public string LocalName { get; set; }

        [StringLength(10)]
        public string Iso639Dash1Code { get; set; }

        [StringLength(8)]
        public string PayPalLanguageCodeId { get; set; }

        [Column("MicroSite_Language")]
        public virtual ICollection<MicroSiteLanguage> MicroSiteLanguage { get; set; }
      
    }
}
