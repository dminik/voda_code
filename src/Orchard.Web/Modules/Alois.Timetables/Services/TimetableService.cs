using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Alois.Timetables.Models;
using Alois.Timetables.Routing;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Routable.Models;

namespace Alois.Timetables.Services {
    [UsedImplicitly]
    public class TimetableService : ITimetableService {
        private readonly IContentManager _contentManager;
        private readonly ITimetableSlugConstraint _TimetableSlugConstraint;

        public TimetableService(IContentManager contentManager, ITimetableSlugConstraint TimetableSlugConstraint) {
            _contentManager = contentManager;
            _TimetableSlugConstraint = TimetableSlugConstraint;
        }

        public TimetablePart Get(string path) {
            return _contentManager.Query<TimetablePart, TimetablePartRecord>()
                .Join<RoutePartRecord>().Where(rr => rr.Path == path)
                .List().FirstOrDefault();
        }

        public ContentItem Get(int id, VersionOptions versionOptions) {
            return _contentManager.Get(id, versionOptions);
        }

        public IEnumerable<TimetablePart> Get() {
            return Get(VersionOptions.Published);
        }

        public IEnumerable<TimetablePart> Get(VersionOptions versionOptions) {
            return _contentManager.Query<TimetablePart, TimetablePartRecord>(versionOptions)
                .Join<RoutePartRecord>()
                .OrderBy(br => br.Title)
                .List();
        }

        public void Delete(ContentItem Timetable) {
            _contentManager.Remove(Timetable);
            _TimetableSlugConstraint.RemoveSlug(Timetable.As<IRoutableAspect>().Path);
        }
    }
}