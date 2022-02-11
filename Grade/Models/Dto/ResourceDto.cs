using System.ComponentModel.DataAnnotations;

namespace Grade.Models.Dto
{
    public class ResourceDto
    {
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        

    }
}
