﻿namespace Grade.Models
{
    [Serializable]
    public class WeeklySection : Section
    {
        public TimeOnly StartAt { get; set; }
        public TimeOnly EndAt { get; set; }
        public DayOfWeek StartDay{ get; set; }
        public DayOfWeek EndDay { get; set; }
        
        
    }
}
