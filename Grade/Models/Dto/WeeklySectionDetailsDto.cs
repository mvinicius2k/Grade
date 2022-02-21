using Grade.Data;
using System.Text.Json.Serialization;

namespace Grade.Models.Dto
{
    [Serializable]
    public class WeeklySectionDetailsDto : WeeklySectionDto
    {
        public PresenterDto[] Presenters { get; set; }
        public ResourceDetailsDto ImageResource { get; set; }
        public string SectionType => nameof(WeeklySection);
    }
}
