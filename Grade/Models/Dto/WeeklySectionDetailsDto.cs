using Grade.Data;

namespace Grade.Models.Dto
{
    public class WeeklySectionDetailsDto : WeeklySectionDto
    {
        public PresenterDto[] Presenters { get; set; }
        public ResourceDetailsDto ImageResource { get; set; }

        
    }
}
