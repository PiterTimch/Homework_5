using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerNovaPost.Data.Entities
{
    [Table("tbl_cities")]
    public class CityEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Ref { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeDescription { get; set; }

        public ICollection<DepartmentEntity> Departments { get; set; } = new List<DepartmentEntity>();
    }
}
