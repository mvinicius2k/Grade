namespace Grade.Models
{
    public class WeeklyProgram2 : ProgramBase
    {
        public TimeOnly StartAt { get; set; }
        public TimeOnly EndAt { get; set; }
        public DayOfWeek StartDay{ get; set; }
        public DayOfWeek EndDay { get; set; }
        

    }
}
