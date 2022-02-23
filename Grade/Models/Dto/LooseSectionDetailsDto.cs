using System.Text.Json.Serialization;

namespace Grade.Models.Dto
{
    public class LooseSectionDetailsDto : LooseSectionDto
    {
        [JsonPropertyName("id")]
        public int SectionId => base.Id;
        public PresenterDetailsDto[] Presenters { get; set; }
        public ResourceDetailsDto ImageResource { get; set; }

    }
}
