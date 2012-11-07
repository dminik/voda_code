using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace Alois.Timetables.Models {
    public class TimetablePartRecord : ContentPartRecord {
        [StringLengthMax]
        public virtual string Description { get; set; }
        public virtual int AppointmentCount { get; set; }
    }
}