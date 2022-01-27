using System;

namespace Miralissa.Server
{
    public record KeysSchedule
    {
        public int Id { get; set; }
        public int IdRosrKeys { get; set; }
        public int DayWeek { get; set; }
        public TimeSpan? TimeBlockSince { get; set; }
        public TimeSpan? TimeBlockTil { get; set; }
    }
}
