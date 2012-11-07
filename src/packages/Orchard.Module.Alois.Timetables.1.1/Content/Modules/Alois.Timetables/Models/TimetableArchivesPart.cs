using Orchard.ContentManagement;

namespace Alois.Timetables.Models {
    /// <summary>
    /// The content part used by the TimetableArchives widget
    /// </summary>
    public class TimetableArchivesPart : ContentPart<TimetableArchivesPartRecord> {
        public string ForTimetable {
            get { return Record.TimetableSlug; }
            set { Record.TimetableSlug = value; }
        }
    }
}
