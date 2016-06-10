using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bigbus.checkout.data.Model
{
    [Table("tb_BornBasketDump")]
    public class BornBasketDump
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string ExternalCookieValue { get; set; }

        public string BasketJsonDump { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
