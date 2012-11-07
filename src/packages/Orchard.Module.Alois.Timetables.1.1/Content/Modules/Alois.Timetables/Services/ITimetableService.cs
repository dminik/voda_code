using System.Collections.Generic;
using Alois.Timetables.Models;
using Orchard.ContentManagement;
using Orchard;

namespace Alois.Timetables.Services {
    public interface ITimetableService : IDependency {
        TimetablePart Get(string path);
        ContentItem Get(int id, VersionOptions versionOptions);
        IEnumerable<TimetablePart> Get();
        IEnumerable<TimetablePart> Get(VersionOptions versionOptions);
        void Delete(ContentItem Timetable);
    }
}