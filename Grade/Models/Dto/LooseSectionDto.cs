using System.ComponentModel.DataAnnotations;

namespace Grade.Models.Dto
{
    public class LooseSectionDto : SectionDto
    {
        [DataType(DataType.DateTime)]
        public DateTime StartAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndAt { get; set; }
    }
}
