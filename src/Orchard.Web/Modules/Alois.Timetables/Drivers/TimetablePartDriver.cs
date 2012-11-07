using JetBrains.Annotations;
using Alois.Timetables.Models;
using Alois.Timetables.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Feeds;
using Orchard.Localization;
using Orchard;

namespace Alois.Timetables.Drivers {
    [UsedImplicitly]
    public class TimetablePartDriver : ContentPartDriver<TimetablePart> {
        public IOrchardServices Services { get; set; }

        private readonly IContentManager _contentManager;
        private readonly ITimetableAppointmentService _TimetableAppointmentService;
        private readonly IFeedManager _feedManager;

        public TimetablePartDriver(
            IOrchardServices services,
            IContentManager contentManager, 
            ITimetableAppointmentService TimetableAppointmentService, 
            IFeedManager feedManager) {
            Services = services;
            _contentManager = contentManager;
            _TimetableAppointmentService = TimetableAppointmentService;
            _feedManager = feedManager;
            T = NullLocalizer.Instance;
        }


        public Localizer T { get; set; }

        protected override string Prefix { get { return ""; } }

        protected override DriverResult Display(TimetablePart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_Timetables_Timetable_Manage",
                             () => shapeHelper.Parts_Timetables_Timetable_Manage(ContentPart: part)),
                ContentShape("Parts_Timetables_Timetable_Description",
                             () => shapeHelper.Parts_Timetables_Timetable_Description(ContentPart: part, Description: part.Description)),
                ContentShape("Parts_Timetables_Timetable_TimetableAppointmentCount",
                             () => shapeHelper.Parts_Timetables_Timetable_TimetableAppointmentCount(ContentPart: part, AppointmentCount: part.AppointmentCount))
                //,
                // todo: (heskew) implement a paging solution that doesn't require Timetable appointments to be tied to the Timetable within the controller
                //ContentShape("Parts_Timetables_TimetableAppointment_List",
                //             () => {
                //                 _feedManager.Register(part);
                //                 var list = shapeHelper.List();
                //                 list.AddRange(_TimetableAppointmentService.Get(part)
                //                                           .Select(bp => _contentManager.BuildDisplay(bp, "Summary")));
                //                 return shapeHelper.Parts_Timetables_TimetableAppointment_List(ContentPart: part, ContentItems: list);
                //             }),
                //ContentShape("Parts_Timetables_TimetableAppointment_List_Admin",
                //             () =>
                //             {
                //                 var list = shapeHelper.List();
                //                 list.AddRange(_TimetableAppointmentService.Get(part, VersionOptions.Latest)
                //                                           .Select(bp => _contentManager.BuildDisplay(bp, "SummaryAdmin")));
                //                 return shapeHelper.Parts_Timetables_TimetableAppointment_List_Admin(ContentPart: part, ContentItems: list);
                //             })
                );
        }

        protected override DriverResult Editor(TimetablePart TimetablePart, dynamic shapeHelper) {
            return ContentShape("Parts_Timetables_Timetable_Fields",
                                () => shapeHelper.EditorTemplate(TemplateName: "Parts.Timetables.Timetable.Fields", Model: TimetablePart, Prefix: Prefix));
        }

        protected override DriverResult Editor(TimetablePart TimetablePart, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(TimetablePart, Prefix, null, null);
            return Editor(TimetablePart, shapeHelper);
        }
    }
}