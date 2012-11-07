using System.Linq;
using Alois.Timetables.Services;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace Alois.Timetables {
    public class AdminMenu : INavigationProvider {
        private readonly ITimetableService _TimetableService;

        public AdminMenu(ITimetableService TimetableService) {
            _TimetableService = TimetableService;
        }

        public Localizer T { get; set; }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T("Timetables"), "2.4", BuildMenu);
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
            var Timetables = _TimetableService.Get();
            var TimetableCount = Timetables.Count();
            var singleTimetable = TimetableCount == 1 ? Timetables.ElementAt(0) : null;

            if (TimetableCount > 0 && singleTimetable == null) {
                menu.Add(T("Manage Timetables"), "3",
                         item => item.Action("List", "TimetableAdmin", new {area = "Alois.Timetables"}).Permission(Permissions.MetaListTimetables));
            }
            else if (singleTimetable != null)
                menu.Add(T("Manage Timetable"), "1.0",
                    item => item.Action("Item", "TimetableAdmin", new { area = "Alois.Timetables", TimetableId = singleTimetable.Id }).Permission(Permissions.MetaListTimetables));

            if (singleTimetable != null)
                menu.Add(T("Create New Appointment"), "1.1",
                         item =>
                         item.Action("Create", "TimetableAppointmentAdmin", new { area = "Alois.Timetables", TimetableId = singleTimetable.Id }).Permission(Permissions.PublishTimetableAppointment));

            menu.Add(T("Create New Timetable"), "1.2",
                     item =>
                     item.Action("Create", "TimetableAdmin", new { area = "Alois.Timetables" }).Permission(Permissions.ManageTimetables));

        }
    }
}