using AutoMapper;
using Grade.Data;
using System.Text.Json.Serialization;

namespace Grade.Models.Dto
{
    public class PresenterDetailsDto : PresenterDto
    {
        [JsonPropertyName("id")]
        public int PresenterId => base.Id;
        public ResourceDetailsDto ImageResource { get; set; }
        public SectionDto[] Sections { get; set; }

       
    }
}
