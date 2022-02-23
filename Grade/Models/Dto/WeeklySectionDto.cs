using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Grade.Models.Dto
{
    public class WeeklySectionDto : SectionDto
    {
        [RegularExpression("^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$")]
        public string StartAt { get; set; }
        [RegularExpression("^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$")]
        public string EndAt { get; set; }


        
        public DayOfWeek StartDay { get; set; }
        public DayOfWeek EndDay { get; set; }


    }
}
