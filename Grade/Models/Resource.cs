using System.ComponentModel.DataAnnotations;

namespace Grade.Models
{
    public class Resource
    {
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        public DateTime UploadedAt { get; set; }

        public ICollection<ProgramBase> ProgramBases { get; set; }
        public ICollection<Presenter> Presenters { get; set; }

    }
}
