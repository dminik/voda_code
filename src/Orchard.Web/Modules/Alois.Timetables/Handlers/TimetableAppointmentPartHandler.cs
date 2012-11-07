using System.Linq;
using System.Web.Routing;
using JetBrains.Annotations;
using Alois.Timetables.Models;
using Alois.Timetables.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Common.Models;
using Orchard.Core.Routable.Models;
using Orchard;

namespace Alois.Timetables.Handlers {
    [UsedImplicitly]
    public class TimetableAppointmentPartHandler : ContentHandler {
        private readonly ITimetableService _TimetableService;
        private readonly ITimetableAppointmentService _TimetableAppointmentService;

        public TimetableAppointmentPartHandler(ITimetableService TimetableService, ITimetableAppointmentService TimetableAppointmentService, RequestContext requestContext) {
            _TimetableService = TimetableService;
            _TimetableAppointmentService = TimetableAppointmentService;

            OnGetDisplayShape<TimetableAppointmentPart>(SetModelProperties);
            OnGetEditorShape<TimetableAppointmentPart>(SetModelProperties);
            OnUpdateEditorShape<TimetableAppointmentPart>(SetModelProperties);

            OnCreated<TimetableAppointmentPart>((context, part) => UpdateTimetableAppointmentCount(part));
            OnPublished<TimetableAppointmentPart>((context, part) => UpdateTimetableAppointmentCount(part));
            OnUnpublished<TimetableAppointmentPart>((context, part) => UpdateTimetableAppointmentCount(part));
            OnVersioned<TimetableAppointmentPart>((context, part, newVersionPart) => UpdateTimetableAppointmentCount(newVersionPart));
            OnRemoved<TimetableAppointmentPart>((context, part) => UpdateTimetableAppointmentCount(part));

            OnRemoved<TimetablePart>(
                (context, b) =>
                TimetableAppointmentService.Get(context.ContentItem.As<TimetablePart>()).ToList().ForEach(
                    TimetableAppointment => context.ContentManager.Remove(TimetableAppointment.ContentItem)));
        }

        private void UpdateTimetableAppointmentCount(TimetableAppointmentPart TimetableAppointmentPart) {
            CommonPart commonPart = TimetableAppointmentPart.As<CommonPart>();
            if (commonPart != null &&
                commonPart.Record.Container != null) {

                TimetablePart TimetablePart = TimetableAppointmentPart.TimetablePart ?? 
                    _TimetableService.Get(commonPart.Record.Container.Id, VersionOptions.Published).As<TimetablePart>();

                // Ensure the "right" set of published appointments for the Timetable is obtained
                TimetablePart.ContentItem.ContentManager.Flush();
                TimetablePart.AppointmentCount = _TimetableAppointmentService.Get(TimetablePart, VersionOptions.Published).Count();
            }
        }

        private static void SetModelProperties(BuildShapeContext context, TimetableAppointmentPart TimetableAppointment) {
            context.Shape.Timetable = TimetableAppointment.TimetablePart;
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            var TimetableAppointment = context.ContentItem.As<TimetableAppointmentPart>();
            
            if (TimetableAppointment == null)
                return;

            context.Metadata.CreateRouteValues = new RouteValueDictionary {
                {"Area", "Alois.Timetables"},
                {"Controller", "TimetableAppointmentAdmin"},
                {"Action", "Create"},
                {"TimetableId", TimetableAppointment.TimetablePart.Id}
            };
            context.Metadata.EditorRouteValues = new RouteValueDictionary {
                {"Area", "Alois.Timetables"},
                {"Controller", "TimetableAppointmentAdmin"},
                {"Action", "Edit"},
                {"postId", context.ContentItem.Id},
                {"TimetableId", TimetableAppointment.TimetablePart.Id}
            };
            context.Metadata.RemoveRouteValues = new RouteValueDictionary {
                {"Area", "Alois.Timetables"},
                {"Controller", "TimetableAppointmentAdmin"},
                {"Action", "Delete"},
                {"postId", context.ContentItem.Id},
                {"TimetableSlug", TimetableAppointment.TimetablePart.As<RoutePart>().Slug}
            };
        }
    }
}