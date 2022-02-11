using System.ComponentModel.DataAnnotations;

namespace Grade.Models.Dto
{
    public class PresenterDto
    {
        internal int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int? ImageId { get; set; }
        public int[] SectionsId { get; set; }
    }
}
