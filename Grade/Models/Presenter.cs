using AutoMapper;
using Grade.Data;
using Grade.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grade.Models
{
    public class Presenter : IId 
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        public int? ResourceId { get; set; }

        public ICollection<Presentation> Presentations { get; set; }

        public Resource Resource { get; set; }

        

        
    }
}
