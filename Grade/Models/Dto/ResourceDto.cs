using System.ComponentModel.DataAnnotations;

namespace Grade.Models.Dto
{
    public class ResourceDto : IResolveTypesPgsql
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Url { get; set; }
        public DateTime UploadedAt { get; set; }

     
        public void ResolveTypesPgsql()
        {
            UploadedAt = UploadedAt.ToUniversalTime();
        }

    }
}
