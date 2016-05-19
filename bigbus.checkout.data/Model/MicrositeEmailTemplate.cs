using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bigbus.checkout.data.Model
{
    [Table("tb_Microsite_Email_Template")]
    public class MicrositeEmailTemplate
    {
        [Key, Column(Order = 0)]
        public Guid MicrositeId { get; set; }

        [Key, Column(Order = 1)]
        public Guid EmailTemplateId {get;set;}
               
        public string LanguageId { get; set; }

        public DateTime Created { get; set; }
    }
}
