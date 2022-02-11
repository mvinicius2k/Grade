namespace Grade.Models.Dto
{
    public class LooseSectionDetailsDto : LooseSectionDto
    {
        public PresenterDto Presenter { get; set; }
        public ResourceDetailsDto ImageResource { get; set; }
    }
}
