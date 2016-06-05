using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bigbus.checkout.data.Model
{
  
    [Table("tb_Navigation")]
    public class Navigation
    {
        public Navigation()
        {
            NavigationItems = new HashSet<NavigationItem>();
        }

        public Guid Id { get; set; }

        [StringLength(50)]
        [Column("MicroSite_Id")]
        public string MicroSiteId { get; set; }

        [StringLength(50)]
        public string Section { get; set; }

        public virtual MicroSite MicroSite { get; set; }

        public virtual ICollection<NavigationItem> NavigationItems { get; set; }
    }
}