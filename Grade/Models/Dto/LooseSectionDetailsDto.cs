namespace Grade.Models.Dto
{
    public class LooseSectionDetailsDto : LooseSectionDto
    {
        public PresenterDto[] Presenters { get; set; }
        public ResourceDetailsDto ImageResource { get; set; }
        
    }
}
