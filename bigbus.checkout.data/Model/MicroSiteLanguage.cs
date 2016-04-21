
namespace bigbus.checkout.data.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tb_MicroSite_Language")]
    public class MicroSiteLanguage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(50)]
        [Column("MicroSite_Id")]
        public string MicroSiteId { get; set; }

        [StringLength(3)]
        [Column("Language_Id")]
        public string LanguageId { get; set; }

        public virtual Language Language { get; set; }

        public virtual MicroSite MicroSite { get; set; }
    }
}
