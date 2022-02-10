using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grade.Models
{
    public abstract class Section
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(200)]
        public string Description { get; set; }
        public bool Active { get; set; }
        
        public string ResourceId { get; set; }

        [NotMapped]
        public ICollection<Apresentation> Apresentations { get; set; }

        public Resource Resource { get; set; }
    }
}
