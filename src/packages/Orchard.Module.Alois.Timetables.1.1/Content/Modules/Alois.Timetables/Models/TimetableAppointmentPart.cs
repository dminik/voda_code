using System;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Common.Models;
using Orchard.Core.Routable.Models;
using Orchard.Security;
using Alois.EventDate.Models;

namespace Alois.Timetables.Models {
    public class TimetableAppointmentPart : ContentPart {

        public TimetablePart TimetablePart {
            get { return this.As<ICommonPart>().Container.As<TimetablePart>(); }
            set { this.As<ICommonPart>().Container = value; }
        }

        public IUser Creator
        {
            get { return this.As<ICommonPart>().Owner; }
            set { this.As<ICommonPart>().Owner = value; }
        }

        public EventDatePart Date
        {
            get { return this.As<EventDatePart>(); }
        }

        public bool IsPublished {
            get { return ContentItem.VersionRecord != null && ContentItem.VersionRecord.Published; }
        }

        public bool HasDraft {
            get {
                return (
                           (ContentItem.VersionRecord != null) && (
                               (ContentItem.VersionRecord.Published == false) ||
                               (ContentItem.VersionRecord.Published && ContentItem.VersionRecord.Latest == false)));
            }
        }

        public bool HasPublished {
            get {
                return IsPublished || ContentItem.ContentManager.Get(Id, VersionOptions.Published) != null;
            }
        }

        public DateTime? PublishedUtc {
            get { return this.As<ICommonPart>().PublishedUtc; }
        }
    }
}