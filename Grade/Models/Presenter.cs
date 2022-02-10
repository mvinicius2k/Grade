using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSwag.Annotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Grade.Models
{
    public class Presenter
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Name { get; set; }
        
        public int ResourceId { get; set; }

        public ICollection<Apresentation> Apresentations { get;  set; }
        
        
        public Resource Resource { get;  set; }
        

    }
}
