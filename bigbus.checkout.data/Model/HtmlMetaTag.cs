using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace bigbus.checkout.data.Model
{
    [Table("tb_HtmlMetaTag")]
    public class HtmlMetaTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string PageType { get; set; }

        public string PageIdentifier { get; set; }

        public string MetaTag { get; set; }

        public string Value { get; set; }
    }
}
