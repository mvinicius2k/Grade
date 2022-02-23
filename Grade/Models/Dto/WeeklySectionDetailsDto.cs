using Grade.Data;
using System.Text.Json.Serialization;

namespace Grade.Models.Dto
{
    [Serializable]
    public class WeeklySectionDetailsDto : WeeklySectionDto
    {
        [JsonPropertyName("id")]
        public int SectionId => base.Id;
        public PresenterDetailsDto[] Presenters { get; set; }
        public ResourceDetailsDto ImageResource { get; set; }
        public string SectionType => nameof(WeeklySection);
    }
}
