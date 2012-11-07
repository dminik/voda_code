using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Alois.Timetables.Models;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Common.Models;
using Orchard.ContentManagement;
using Orchard.Core.Routable.Models;
using Orchard.Core.Routable.Services;
using Orchard.Data;
using Orchard.Tasks.Scheduling;

namespace Alois.Timetables.Services {
    [UsedImplicitly]
    public class TimetableAppointmentService : ITimetableAppointmentService {
        private readonly IContentManager _contentManager;
        private readonly IRepository<TimetablePartArchiveRecord> _TimetableArchiveRepository;
        private readonly IPublishingTaskManager _publishingTaskManager;

        public TimetableAppointmentService(IContentManager contentManager, IRepository<TimetablePartArchiveRecord> TimetableArchiveRepository, IPublishingTaskManager publishingTaskManager) {
            _contentManager = contentManager;
            _TimetableArchiveRepository = TimetableArchiveRepository;
            _publishingTaskManager = publishingTaskManager;
        }

        public TimetableAppointmentPart Get(TimetablePart TimetablePart, string slug) {
            return Get(TimetablePart, slug, VersionOptions.Published);
        }

        public TimetableAppointmentPart Get(TimetablePart TimetablePart, string slug, VersionOptions versionOptions) {
            var postSlug = TimetablePart.As<IRoutableAspect>().GetChildPath(slug);
            return
                _contentManager.Query(versionOptions, "TimetableAppointment").Join<RoutePartRecord>().Where(rr => rr.Path == postSlug).
                    Join<CommonPartRecord>().Where(cr => cr.Container == TimetablePart.Record.ContentItemRecord).List().
                    SingleOrDefault().As<TimetableAppointmentPart>();
        }

        public TimetableAppointmentPart Get(int id) {
            return Get(id, VersionOptions.Published);
        }

        public TimetableAppointmentPart Get(int id, VersionOptions versionOptions) {
            return _contentManager.Get<TimetableAppointmentPart>(id, versionOptions);
        }

        public IEnumerable<TimetableAppointmentPart> Get(TimetablePart TimetablePart) {
            return Get(TimetablePart, VersionOptions.Published);
        }

        public IEnumerable<TimetableAppointmentPart> Get(TimetablePart TimetablePart, VersionOptions versionOptions) {
            return GetTimetableQuery(TimetablePart, versionOptions).List().Select(ci => ci.As<TimetableAppointmentPart>());
        }

        public IEnumerable<TimetableAppointmentPart> Get(TimetablePart TimetablePart, int skip, int count) {
            return Get(TimetablePart, skip, count, VersionOptions.Published);
        }

        public IEnumerable<TimetableAppointmentPart> Get(TimetablePart TimetablePart, int skip, int count, VersionOptions versionOptions) {
            return GetTimetableQuery(TimetablePart, versionOptions).Slice(skip, count).ToList().Select(ci => ci.As<TimetableAppointmentPart>());
        }

        public int AppointmentCount(TimetablePart TimetablePart) {
            return AppointmentCount(TimetablePart, VersionOptions.Published);
        }

        public int AppointmentCount(TimetablePart TimetablePart, VersionOptions versionOptions) {
            return GetTimetableQuery(TimetablePart, versionOptions).Count();
        }

        public IEnumerable<TimetableAppointmentPart> Get(TimetablePart TimetablePart, ArchiveData archiveData) {
            var query = GetTimetableQuery(TimetablePart, VersionOptions.Published);

            if (archiveData.Day > 0) {
                var dayDate = new DateTime(archiveData.Year, archiveData.Month, archiveData.Day);

                query = query.Where(cr => cr.CreatedUtc >= dayDate && cr.CreatedUtc < dayDate.AddDays(1));
            }
            else if (archiveData.Month > 0)
            {
                var monthDate = new DateTime(archiveData.Year, archiveData.Month, 1);

                query = query.Where(cr => cr.CreatedUtc >= monthDate && cr.CreatedUtc < monthDate.AddMonths(1));
            }
            else {
                var yearDate = new DateTime(archiveData.Year, 1, 1);

                query = query.Where(cr => cr.CreatedUtc >= yearDate && cr.CreatedUtc < yearDate.AddYears(1));
            }

            return query.List().Select(ci => ci.As<TimetableAppointmentPart>());
        }

        public IEnumerable<KeyValuePair<ArchiveData, int>> GetArchives(TimetablePart TimetablePart) {
            var query = 
                from bar in _TimetableArchiveRepository.Table
                where bar.TimetablePart == TimetablePart.Record
                orderby bar.Year descending, bar.Month descending
                select bar;

            return
                query.ToList().Select(
                    bar =>
                    new KeyValuePair<ArchiveData, int>(new ArchiveData(string.Format("{0}/{1}", bar.Year, bar.Month)),
                                                       bar.AppointmentCount));
        }

        public void Delete(TimetableAppointmentPart TimetableAppointmentPart) {
            _publishingTaskManager.DeleteTasks(TimetableAppointmentPart.ContentItem);
            _contentManager.Remove(TimetableAppointmentPart.ContentItem);
        }

        public void Publish(TimetableAppointmentPart TimetableAppointmentPart) {
            _publishingTaskManager.DeleteTasks(TimetableAppointmentPart.ContentItem);
            _contentManager.Publish(TimetableAppointmentPart.ContentItem);
        }

        public void Publish(TimetableAppointmentPart TimetableAppointmentPart, DateTime scheduledPublishUtc) {
            _publishingTaskManager.Publish(TimetableAppointmentPart.ContentItem, scheduledPublishUtc);
        }

        public void Unpublish(TimetableAppointmentPart TimetableAppointmentPart) {
            _contentManager.Unpublish(TimetableAppointmentPart.ContentItem);
        }

        public DateTime? GetScheduledPublishUtc(TimetableAppointmentPart TimetableAppointmentPart) {
            var task = _publishingTaskManager.GetPublishTask(TimetableAppointmentPart.ContentItem);
            return (task == null ? null : task.ScheduledUtc);
        }

        private IContentQuery<ContentItem, CommonPartRecord> GetTimetableQuery(ContentPart<TimetablePartRecord> Timetable, VersionOptions versionOptions) {
            return
                _contentManager.Query(versionOptions, "TimetableAppointment").Join<CommonPartRecord>().Where(
                    cr => cr.Container == Timetable.Record.ContentItemRecord).OrderByDescending(cr => cr.CreatedUtc);
        }
    }
}