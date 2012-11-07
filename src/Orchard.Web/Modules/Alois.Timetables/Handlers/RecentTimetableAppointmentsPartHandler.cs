using JetBrains.Annotations;
using Alois.Timetables.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Alois.Timetables.Handlers {
    [UsedImplicitly]
    public class RecentTimetableAppointmentsPartHandler : ContentHandler {
        public RecentTimetableAppointmentsPartHandler(IRepository<RecentTimetableAppointmentsPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}