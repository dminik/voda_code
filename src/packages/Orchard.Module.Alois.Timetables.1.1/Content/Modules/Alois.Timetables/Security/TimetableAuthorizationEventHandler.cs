using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Security;
using Orchard.Security.Permissions;

namespace Alois.Timetables.Security {
    [UsedImplicitly]
    public class TimetableAuthorizationEventHandler : IAuthorizationServiceEventHandler {
        public void Checking(CheckAccessContext context) { }
        public void Complete(CheckAccessContext context) { }

        public void Adjust(CheckAccessContext context) {
            if (!context.Granted &&
                context.Content.Is<ICommonPart>()) {
                if (OwnerVariationExists(context.Permission) &&
                    HasOwnership(context.User, context.Content)) {
                    context.Adjusted = true;
                    context.Permission = GetOwnerVariation(context.Permission);
                }
            }
        }

        private static bool HasOwnership(IUser user, IContent content) {
            if (user == null || content == null)
                return false;

            var common = content.As<ICommonPart>();
            if (common == null || common.Owner == null)
                return false;

            return user.Id == common.Owner.Id;
        }

        private static bool OwnerVariationExists(Permission permission) {
            return GetOwnerVariation(permission) != null;
        }

        private static Permission GetOwnerVariation(Permission permission) {
            if (permission.Name == Permissions.PublishTimetableAppointment.Name)
                return Permissions.PublishOwnTimetableAppointment;
            if (permission.Name == Permissions.EditTimetableAppointment.Name)
                return Permissions.EditOwnTimetableAppointment;
            if (permission.Name == Permissions.DeleteTimetableAppointment.Name)
                return Permissions.DeleteOwnTimetableAppointment;
            return null;
        }
    }
}