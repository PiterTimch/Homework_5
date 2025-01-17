using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerNovaPost.Data.Entities
{
    [Table("tbl_areas")]
    public class AreaEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Ref { get; set; }

        [Required]
        [StringLength(50)]
        public string AreasCenter { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        public ICollection<CityEntity> Cities { get; set; } = new List<CityEntity>();
    }
}
