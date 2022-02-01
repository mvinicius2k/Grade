using System.ComponentModel.DataAnnotations;

namespace Grade.Models
{
    public abstract class ProgramBase
    {
        public int Id { get; set; }

        [Required][MaxLength(100)]
        public string Name { get; set; }
        [Required][MaxLength(200)]
        public string Description { get; set; }
        public bool Active { get; set; }
        
        public int? ResourceId { get; set; }
        
        public ICollection<Apresentation> Presenters { get; set; }
    }
}
