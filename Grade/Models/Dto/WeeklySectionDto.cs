namespace Grade.Models.Dto
{
    public class WeeklySectionDto : SectionDto
    {
        public TimeOnly StartAt { get; set; }
        public TimeOnly EndAt { get; set; }
        public DayOfWeek StartDay { get; set; }
        public DayOfWeek EndDay { get; set; }
    }
}
