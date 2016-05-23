using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bigbus.checkout.data.Model
{
    [Table("tb_ContactData")]
    public class ContactData
    {
        public Guid Id { get; set; }

        [StringLength(50)]
        [Column("MicroSite_Id")]
        public string MicroSiteId { get; set; }

        [StringLength(50)]
        public string Page { get; set; }

        [StringLength(500)]
        public string Email { get; set; }
    }
}

