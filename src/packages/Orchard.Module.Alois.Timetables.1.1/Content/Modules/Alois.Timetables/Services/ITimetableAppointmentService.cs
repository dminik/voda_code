using System;
using System.Collections.Generic;
using Alois.Timetables.Models;
using Orchard.ContentManagement;
using Orchard;

namespace Alois.Timetables.Services {
    public interface ITimetableAppointmentService : IDependency {
        TimetableAppointmentPart Get(TimetablePart TimetablePart, string slug);
        TimetableAppointmentPart Get(TimetablePart TimetablePart, string slug, VersionOptions versionOptions);
        TimetableAppointmentPart Get(int id);
        TimetableAppointmentPart Get(int id, VersionOptions versionOptions);
        IEnumerable<TimetableAppointmentPart> Get(TimetablePart TimetablePart);
        IEnumerable<TimetableAppointmentPart> Get(TimetablePart TimetablePart, VersionOptions versionOptions);
        IEnumerable<TimetableAppointmentPart> Get(TimetablePart TimetablePart, ArchiveData archiveData);
        IEnumerable<TimetableAppointmentPart> Get(TimetablePart TimetablePart, int skip, int count);
        IEnumerable<TimetableAppointmentPart> Get(TimetablePart TimetablePart, int skip, int count, VersionOptions versionOptions);
        int AppointmentCount(TimetablePart TimetablePart);
        int AppointmentCount(TimetablePart TimetablePart, VersionOptions versionOptions);
        IEnumerable<KeyValuePair<ArchiveData, int>> GetArchives(TimetablePart TimetablePart);
        void Delete(TimetableAppointmentPart TimetableAppointmentPart);
        void Publish(TimetableAppointmentPart TimetableAppointmentPart);
        void Publish(TimetableAppointmentPart TimetableAppointmentPart, DateTime scheduledPublishUtc);
        void Unpublish(TimetableAppointmentPart TimetableAppointmentPart);
        DateTime? GetScheduledPublishUtc(TimetableAppointmentPart TimetableAppointmentPart);
    }
}