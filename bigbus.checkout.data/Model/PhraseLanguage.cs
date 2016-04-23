using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bigbus.checkout.data.Model
{

    [Table("tb_Phrase_Language")]
    public class PhraseLanguage
    {
        public Guid Id { get; set; }

        [StringLength(250)]
        [Column("Phrase_Id")]
        public string PhraseId { get; set; }

        [StringLength(3)]
        [Column("Language_Id")]
        public string LanguageId { get; set; }

        [StringLength(500)]
        public string Translation { get; set; }

        public virtual Language Language { get; set; }

        public virtual Phrase Phrase { get; set; }
    }
}
