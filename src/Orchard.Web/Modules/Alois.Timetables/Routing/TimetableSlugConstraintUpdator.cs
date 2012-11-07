using System.Linq;
using JetBrains.Annotations;
using Alois.Timetables.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Environment;
using Orchard.Tasks;

namespace Alois.Timetables.Routing {
    [UsedImplicitly]
    public class TimetableSlugConstraintUpdator : IOrchardShellEvents, IBackgroundTask {
        private readonly ITimetableSlugConstraint _TimetableSlugConstraint;
        private readonly ITimetableService _TimetableService;

        public TimetableSlugConstraintUpdator(ITimetableSlugConstraint TimetableSlugConstraint, ITimetableService TimetableService) {
            _TimetableSlugConstraint = TimetableSlugConstraint;
            _TimetableService = TimetableService;
        }
        
        void IOrchardShellEvents.Activated() {
            Refresh();
        }

        void IOrchardShellEvents.Terminating() {
        }

        void IBackgroundTask.Sweep() {
            Refresh();
        }

        private void Refresh() {
            _TimetableSlugConstraint.SetSlugs(_TimetableService.Get().Select(b => b.As<IRoutableAspect>().Path));
        }
    }
}