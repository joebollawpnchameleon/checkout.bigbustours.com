using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace bigbus.checkout.data.Model
{
   

    [Table("tb_ImageMetaData")]
    public class ImageMetaData
    {
        public ImageMetaData()
        {
            Image = new HashSet<Image>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        [StringLength(100)]
        public string Type { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(200)]
        public string FileName { get; set; }

        [StringLength(500)]
        public string AltText { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        [Column("Image_Id")]
        public Guid? ImageId { get; set; }

        [StringLength(200)]
        public string Tags { get; set; }

        [Column("ImageFolder_Id")]
        public Guid? ImageFolderId { get; set; }

        public virtual ICollection<Image> Image { get; set; }
    }
}
