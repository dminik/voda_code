using System.Web.Mvc;
using Alois.Timetables.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Routable.Services;
using Orchard.Mvc.Extensions;
using Orchard;

namespace Alois.Timetables.Extensions {
    public static class UrlHelperExtensions {
        public static string Timetables(this UrlHelper urlHelper) {
            return urlHelper.Action("List", "Timetable", new {area = "Alois.Timetables"});
        }

        public static string TimetablesForAdmin(this UrlHelper urlHelper) {
            return urlHelper.Action("List", "TimetableAdmin", new {area = "Alois.Timetables"});
        }

        public static string Timetable(this UrlHelper urlHelper, TimetablePart TimetablePart) {
            return urlHelper.Action("Item", "Timetable", new { TimetableSlug = TimetablePart.As<IRoutableAspect>().Path, area = "Alois.Timetables" });
        }

        public static string TimetableLiveWriterManifest(this UrlHelper urlHelper, TimetablePart TimetablePart) {
            return urlHelper.AbsoluteAction(() => urlHelper.Action("Manifest", "LiveWriter", new { area = "XmlRpc" }));
        }

        public static string TimetableRsd(this UrlHelper urlHelper, TimetablePart TimetablePart) {
            return urlHelper.AbsoluteAction(() => urlHelper.Action("Rsd", "RemoteTimetablePublishing", new { TimetableSlug = TimetablePart.As<IRoutableAspect>().Path, area = "Alois.Timetables" }));
        }

        public static string TimetableArchiveYear(this UrlHelper urlHelper, TimetablePart TimetablePart, int year) {
            return urlHelper.Action("ListByArchive", "TimetableAppointment", new { TimetableSlug = TimetablePart.As<IRoutableAspect>().Path, archiveData = year.ToString(), area = "Alois.Timetables" });
        }

        public static string TimetableArchiveMonth(this UrlHelper urlHelper, TimetablePart TimetablePart, int year, int month) {
            return urlHelper.Action("ListByArchive", "TimetableAppointment", new { TimetableSlug = TimetablePart.As<IRoutableAspect>().Path, archiveData = string.Format("{0}/{1}", year, month), area = "Alois.Timetables" });
        }

        public static string TimetableArchiveDay(this UrlHelper urlHelper, TimetablePart TimetablePart, int year, int month, int day) {
            return urlHelper.Action("ListByArchive", "TimetableAppointment", new { TimetableSlug = TimetablePart.As<IRoutableAspect>().Path, archiveData = string.Format("{0}/{1}/{2}", year, month, day), area = "Alois.Timetables" });
        }

        public static string TimetableForAdmin(this UrlHelper urlHelper, TimetablePart TimetablePart) {
            return urlHelper.Action("Item", "TimetableAdmin", new { TimetableId = TimetablePart.Id, area = "Alois.Timetables" });
        }

        public static string TimetableCreate(this UrlHelper urlHelper) {
            return urlHelper.Action("Create", "TimetableAdmin", new { area = "Alois.Timetables" });
        }

        public static string TimetableEdit(this UrlHelper urlHelper, TimetablePart TimetablePart) {
            return urlHelper.Action("Edit", "TimetableAdmin", new { TimetableId = TimetablePart.Id, area = "Alois.Timetables" });
        }

        public static string TimetableRemove(this UrlHelper urlHelper, TimetablePart TimetablePart) {
            return urlHelper.Action("Remove", "TimetableAdmin", new { TimetableId = TimetablePart.Id, area = "Alois.Timetables" });
        }

        public static string TimetableAppointmentCreate(this UrlHelper urlHelper, TimetablePart TimetablePart) {
            return urlHelper.Action("Create", "TimetableAppointmentAdmin", new { TimetableId = TimetablePart.Id, area = "Alois.Timetables" });
        }

        public static string TimetableAppointment(this UrlHelper urlHelper, TimetableAppointmentPart TimetableAppointmentPart) {
            return urlHelper.Action("Item", "TimetableAppointment", new { TimetableSlug = TimetableAppointmentPart.TimetablePart.As<IRoutableAspect>().Path, postSlug = TimetableAppointmentPart.As<IRoutableAspect>().GetEffectiveSlug(), area = "Alois.Timetables" });
        }

        public static string TimetableAppointmentEdit(this UrlHelper urlHelper, TimetableAppointmentPart TimetableAppointmentPart) {
            return urlHelper.Action("Edit", "TimetableAppointmentAdmin", new { TimetableId = TimetableAppointmentPart.TimetablePart.Id, postId = TimetableAppointmentPart.Id, area = "Alois.Timetables" });
        }

        public static string TimetableAppointmentDelete(this UrlHelper urlHelper, TimetableAppointmentPart TimetableAppointmentPart) {
            return urlHelper.Action("Delete", "TimetableAppointmentAdmin", new { TimetableId = TimetableAppointmentPart.TimetablePart.Id, postId = TimetableAppointmentPart.Id, area = "Alois.Timetables" });
        }

        public static string TimetableAppointmentPublish(this UrlHelper urlHelper, TimetableAppointmentPart TimetableAppointmentPart) {
            return urlHelper.Action("Publish", "TimetableAppointmentAdmin", new { TimetableId = TimetableAppointmentPart.TimetablePart.Id, postId = TimetableAppointmentPart.Id, area = "Alois.Timetables" });
        }

        public static string TimetableAppointmentUnpublish(this UrlHelper urlHelper, TimetableAppointmentPart TimetableAppointmentPart) {
            return urlHelper.Action("Unpublish", "TimetableAppointmentAdmin", new { TimetableId = TimetableAppointmentPart.TimetablePart.Id, postId = TimetableAppointmentPart.Id, area = "Alois.Timetables" });
        }
    }
}