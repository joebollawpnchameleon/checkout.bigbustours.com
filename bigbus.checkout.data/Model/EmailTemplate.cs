using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bigbus.checkout.data.Model
{
    [Table("tb_EmailTemplate")]
    public class EmailTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string LanguageId { get; set; }

        public string Name { get; set; }

        public string ContentFile { get; set; }

        public string Title { get; set; }

        public DateTime Created { get; set; }
       
        public DateTime? LastModified { get; set; }
    }
}
