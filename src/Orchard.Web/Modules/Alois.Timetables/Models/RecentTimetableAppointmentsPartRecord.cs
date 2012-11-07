using Orchard.ContentManagement.Records;

namespace Alois.Timetables.Models {
    public class RecentTimetableAppointmentsPartRecord : ContentPartRecord {
        public virtual string TimetableSlug { get; set; }
        public virtual int Count { get; set; }
    }
}
