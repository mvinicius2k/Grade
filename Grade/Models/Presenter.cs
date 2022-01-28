﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grade.Models
{
    public class Presenter
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        public int? ResourceId { get; set; }

        public ICollection<Apresentation> Apresentations { get; set; }

    }
}
