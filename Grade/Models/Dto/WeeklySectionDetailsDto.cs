namespace Grade.Models.Dto
{
    public class WeeklySectionDetailsDto : WeeklySectionDto
    {
        public PresenterDto Presenter { get; set; }
        public ResourceDetailsDto ImageResource { get; set; }
    }
}
