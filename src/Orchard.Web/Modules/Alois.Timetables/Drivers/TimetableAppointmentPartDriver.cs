using JetBrains.Annotations;
using Alois.Timetables.Models;
using Alois.Timetables.Extensions;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Feeds;
using Orchard.Localization;
using Orchard;

namespace Alois.Timetables.Drivers {
    [UsedImplicitly]
    public class TimetableAppointmentPartDriver : ContentPartDriver<TimetableAppointmentPart> {
        private readonly IFeedManager _feedManager;
        public IOrchardServices Services { get; set; }

        public TimetableAppointmentPartDriver(IOrchardServices services, IFeedManager feedManager) {
            _feedManager = feedManager;
            Services = services;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix { get { return ""; } }

        protected override DriverResult Display(TimetableAppointmentPart part, string displayType, dynamic shapeHelper) {
            if (displayType.StartsWith("Detail"))
                _feedManager.Register(part.TimetablePart);

            return null;
        }
    }
}