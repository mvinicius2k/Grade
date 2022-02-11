using System.ComponentModel.DataAnnotations;

namespace Grade.Models.Dto
{
    public abstract class SectionDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        public bool Active { get; set; }

        public int? ImageId { get; set; }

        
    }
}
