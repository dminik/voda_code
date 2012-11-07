using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Common.Models;
using Orchard.Core.Routable.Models;
using Orchard.Security;

namespace Alois.Timetables.Models {
    public class TimetablePart : ContentPart<TimetablePartRecord> {
        public string Name {
            get { return this.As<RoutePart>().Title; }
            set { this.As<RoutePart>().Title = value; }
        }

        public string Description {
            get { return Record.Description; }
            set { Record.Description = value; }
        }

        public int AppointmentCount {
            get { return Record.AppointmentCount; }
            set { Record.AppointmentCount = value; }
        }
        
        public IUser Creator
        {
            get { return this.As<ICommonPart>().Owner; }
            set { this.As<ICommonPart>().Owner = value; }
        }
    }
}