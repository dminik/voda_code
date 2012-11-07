using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace Alois.Timetables {
    public class Permissions : IPermissionProvider {
        public static readonly Permission ManageTimetables = new Permission { Description = "Manage Timetables", Name = "ManageTimetables" };

        public static readonly Permission PublishTimetableAppointment = new Permission { Description = "Publish or unpublish Timetable appointment for others", Name = "PublishTimetableAppointment", ImpliedBy = new[] { ManageTimetables } };
        public static readonly Permission PublishOwnTimetableAppointment = new Permission { Description = "Publish or unpublish own Timetable appointment", Name = "PublishOwnTimetableAppointment", ImpliedBy = new[] { PublishTimetableAppointment } };
        public static readonly Permission EditTimetableAppointment = new Permission { Description = "Edit any Timetable appointments", Name = "EditTimetableAppointment", ImpliedBy = new[] { PublishTimetableAppointment } };
        public static readonly Permission EditOwnTimetableAppointment = new Permission { Description = "Edit own Timetable appointments", Name = "EditOwnTimetableAppointment", ImpliedBy = new[] { EditTimetableAppointment, PublishOwnTimetableAppointment } };
        public static readonly Permission DeleteTimetableAppointment = new Permission { Description = "Delete Timetable appointment for others", Name = "DeleteTimetableAppointment", ImpliedBy = new[] { ManageTimetables } };
        public static readonly Permission DeleteOwnTimetableAppointment = new Permission { Description = "Delete own Timetable appointment", Name = "DeleteOwnTimetableAppointment", ImpliedBy = new[] { DeleteTimetableAppointment } };

        public static readonly Permission MetaListTimetables = new Permission { ImpliedBy = new[] { EditTimetableAppointment, PublishTimetableAppointment, DeleteTimetableAppointment } };
        public static readonly Permission MetaListOwnTimetables = new Permission { ImpliedBy = new[] { EditOwnTimetableAppointment, PublishOwnTimetableAppointment, DeleteOwnTimetableAppointment } };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                ManageTimetables,
                EditOwnTimetableAppointment,
                EditTimetableAppointment,
                PublishOwnTimetableAppointment,
                PublishTimetableAppointment,
                DeleteOwnTimetableAppointment,
                DeleteTimetableAppointment,
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {ManageTimetables}
                },
                new PermissionStereotype {
                    Name = "Editor",
                    Permissions = new[] {PublishTimetableAppointment,EditTimetableAppointment,DeleteTimetableAppointment}
                },
                new PermissionStereotype {
                    Name = "Moderator",
                },
                new PermissionStereotype {
                    Name = "Author",
                    Permissions = new[] {PublishOwnTimetableAppointment,EditOwnTimetableAppointment,DeleteOwnTimetableAppointment}
                },
                new PermissionStereotype {
                    Name = "Contributor",
                    Permissions = new[] {EditOwnTimetableAppointment}
                },
            };
        }

    }
}


