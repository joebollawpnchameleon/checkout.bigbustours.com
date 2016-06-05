using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
   

namespace bigbus.checkout.data.Model
{
   

    [Table("tb_DiallingCode")]
    public class DiallingCode
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Code { get; set; }
    }
}