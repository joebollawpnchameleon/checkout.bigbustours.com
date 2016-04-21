using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace bigbus.checkout.data.Model
{


    [Table("tb_ImageFolder")]
    public class ImageFolder
    {
        public ImageFolder()
        {
            Image = new HashSet<Image>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string FolderName { get; set; }

        [Column("ParentFolder_Id")]
        public Guid? ParentFolderId { get; set; }

        public virtual ICollection<Image> Image { get; set; }

    }
}
