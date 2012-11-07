using Alois.Timetables.Models;
using Alois.Timetables.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard;

namespace Alois.Timetables.Drivers {
    public class TimetableArchivesPartDriver : ContentPartDriver<TimetableArchivesPart> {
        private readonly ITimetableService _TimetableService;
        private readonly ITimetableAppointmentService _TimetableAppointmentService;

        public TimetableArchivesPartDriver(ITimetableService TimetableService, ITimetableAppointmentService TimetableAppointmentService) {
            _TimetableService = TimetableService;
            _TimetableAppointmentService = TimetableAppointmentService;
        }

        protected override DriverResult Display(TimetableArchivesPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Timetables_TimetableArchives",
                                () => {
                                    TimetablePart Timetable = null;
                                    if (!string.IsNullOrWhiteSpace(part.ForTimetable))
                                        Timetable = _TimetableService.Get(part.ForTimetable);

                                    if (Timetable == null)
                                        return null;

                                    return shapeHelper.Parts_Timetables_TimetableArchives(ContentItem: part.ContentItem, Timetable: Timetable, Archives: _TimetableAppointmentService.GetArchives(Timetable));
                                });
        }

        protected override DriverResult Editor(TimetableArchivesPart part, dynamic shapeHelper) {
            return ContentShape("Parts_Timetables_TimetableArchives_Edit",
                                () => shapeHelper.EditorTemplate(TemplateName: "Parts.Timetables.TimetableArchives", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(TimetableArchivesPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}