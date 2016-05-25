using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bigbus.checkout.data.Model
{
    [Table("tb_Microsite_Email_Template")]
    public class MicrositeEmailTemplate
    {
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string MicrositeId { get; set; }

        [Key, Column(Order = 1)]
        public Guid EmailTemplateId {get;set;}
               
        public string LanguageId { get; set; }

        public DateTime Created { get; set; }

        public string TrustPilotLink { get; set; }

        public string TripAdvisorLink { get; set; }
    }
}
