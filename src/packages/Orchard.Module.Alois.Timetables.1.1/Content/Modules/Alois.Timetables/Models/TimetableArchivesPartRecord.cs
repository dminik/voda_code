using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement.Records;

namespace Alois.Timetables.Models {
    /// <summary>
    /// The content part used by the TimetableArchives widget
    /// </summary>
    public class TimetableArchivesPartRecord : ContentPartRecord {
        public const ushort DefaultTimetableSlugLength = 255;

        [StringLength(DefaultTimetableSlugLength)]
        [Required]
        public virtual string TimetableSlug { get; set; }
    }
}
