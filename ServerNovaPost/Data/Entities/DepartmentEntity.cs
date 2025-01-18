using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerNovaPost.Data.Entities
{
    [Table("tbl_departments")]
    public class DepartmentEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Ref { get; set; }

        [Required]
        [StringLength(50)]
        public string CityRef { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone { get; set; }
    }
}
