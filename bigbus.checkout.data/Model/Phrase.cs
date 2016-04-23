using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bigbus.checkout.data.Model
{
    [Table("tb_Phrase")]
    public class Phrase
    {
        public Phrase()
        {
            PhraseLanguage = new HashSet<PhraseLanguage>();
        }

        [StringLength(250)]
        public string Id { get; set; }

        public virtual ICollection<PhraseLanguage> PhraseLanguage { get; set; }
    }
}
