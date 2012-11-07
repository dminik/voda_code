using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;

namespace Alois.Timetables.Models {
    public class RecentTimetableAppointmentsPart : ContentPart<RecentTimetableAppointmentsPartRecord> {
        public string ForTimetable {
            get { return Record.TimetableSlug; }
            set { Record.TimetableSlug = value; }
        }

        [Required]
        public int Count {
            get { return Record.Count; }
            set { Record.Count = value; }
        }
    }
}
