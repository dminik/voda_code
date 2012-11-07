namespace Alois.Timetables.Models {
    public class TimetablePartArchiveRecord {
        public virtual int Id { get; set; }
        public virtual TimetablePartRecord TimetablePart { get; set; }
        public virtual int Year { get; set; }
        public virtual int Month { get; set; }
        public virtual int AppointmentCount { get; set; }
    }
}