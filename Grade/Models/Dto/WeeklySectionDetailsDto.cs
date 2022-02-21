using Grade.Data;

namespace Grade.Models.Dto
{
    [Serializable]
    public class WeeklySectionDetailsDto : WeeklySectionDto, IPolymorphicSerialization<ESectionDtoDetailsType>
    {
        public PresenterDto[] Presenters { get; set; }
        public ResourceDetailsDto ImageResource { get; set; }
        public ESectionDtoDetailsType DerivatedBy { get ; set; }
    }
}
