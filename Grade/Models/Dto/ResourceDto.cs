using System.ComponentModel.DataAnnotations;

namespace Grade.Models.Dto
{
    public class ResourceDto
    {
        internal int Id { get; set; }
        [Required]
        public string Url { get; set; }
        

    }
}
