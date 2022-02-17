using Grade.Data;

namespace Grade.Models.Dto
{
    [Serializable]
    public class WeeklySectionDetailsDto : WeeklySectionDto
    {
        public PresenterDto[] Presenters { get; set; }
        public ResourceDetailsDto ImageResource { get; set; }

        
    }
}
