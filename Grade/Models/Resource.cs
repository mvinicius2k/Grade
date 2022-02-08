using System.ComponentModel.DataAnnotations;

namespace Grade.Models
{
    public class Resource : IResolveTypesPgsql
    {
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        public DateTime UploadedAt { get; set; }

        public ICollection<Section> Sections { get; set; }
        public ICollection<Presenter> Presenters { get; set; }

        public void ResolveTypesPgsql()
        {
            UploadedAt = UploadedAt.ToUniversalTime();
        }
    }
}
