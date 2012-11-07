using System.Collections.Generic;
using System.Linq;
using Alois.Timetables.Models;
using Alois.Timetables.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;
using Orchard;

namespace Alois.Timetables.Drivers {
    public class RecentTimetableAppointmentsPartDriver : ContentPartDriver<RecentTimetableAppointmentsPart> {
        private readonly ITimetableService _TimetableService;
        private readonly IContentManager _contentManager;

        public RecentTimetableAppointmentsPartDriver(ITimetableService TimetableService, IContentManager contentManager) {
            _TimetableService = TimetableService;
            _contentManager = contentManager;
        }

        protected override DriverResult Display(RecentTimetableAppointmentsPart part, string displayType, dynamic shapeHelper) {
            IEnumerable<TimetableAppointmentPart> TimetableAppointments;

            TimetablePart Timetable = null;
            if (!string.IsNullOrWhiteSpace(part.ForTimetable))
                Timetable = _TimetableService.Get(part.ForTimetable);

            if (Timetable != null) {
                TimetableAppointments = _contentManager.Query(VersionOptions.Published, "TimetableAppointment")
                    .Join<CommonPartRecord>().Where(cr => cr.Container == Timetable.Record.ContentItemRecord)
                    .OrderByDescending(cr => cr.CreatedUtc)
                    .Slice(0, part.Count)
                    .Select(ci => ci.As<TimetableAppointmentPart>());
            }
            else {
                TimetableAppointments = _contentManager.Query(VersionOptions.Published, "TimetableAppointment")
                    .Join<CommonPartRecord>()
                    .OrderByDescending(cr => cr.CreatedUtc)
                    .Slice(0, part.Count)
                    .Select(ci => ci.As<TimetableAppointmentPart>());
            }

            var list = shapeHelper.List();
            list.AddRange(TimetableAppointments.Select(bp => _contentManager.BuildDisplay(bp, "Summary")));

            var TimetableAppointmentList = shapeHelper.Parts_Timetables_TimetableAppointment_List(ContentPart: part, ContentItems: list);

            return ContentShape(shapeHelper.Parts_Timetables_RecentTimetableAppointments(ContentItem: part.ContentItem, ContentItems: TimetableAppointmentList));
        }

        protected override DriverResult Editor(RecentTimetableAppointmentsPart part, dynamic shapeHelper) {
            return ContentShape("Parts_Timetables_RecentTimetableAppointments_Edit",
                                () => shapeHelper.EditorTemplate(TemplateName: "Parts.Timetables.RecentTimetableAppointments", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(RecentTimetableAppointmentsPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}