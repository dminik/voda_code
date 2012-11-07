using JetBrains.Annotations;
using Alois.Timetables.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Alois.Timetables.Handlers {
    [UsedImplicitly]
    public class TimetableArchivesPartHandler : ContentHandler {
        public TimetableArchivesPartHandler(IRepository<TimetableArchivesPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}