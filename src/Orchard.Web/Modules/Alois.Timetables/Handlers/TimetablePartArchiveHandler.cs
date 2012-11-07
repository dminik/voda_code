using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Alois.Timetables.Models;
using Alois.Timetables.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Common.Models;
using Orchard.Data;
using Orchard;

namespace Alois.Timetables.Handlers {
    [UsedImplicitly]
    public class TimetablePartArchiveHandler : ContentHandler {
        public TimetablePartArchiveHandler(IRepository<TimetablePartArchiveRecord> TimetableArchiveRepository, ITimetableAppointmentService TimetableAppointmentService) {
            OnPublished<TimetableAppointmentPart>((context, bp) => RecalculateTimetableArchive(TimetableArchiveRepository, TimetableAppointmentService, bp));
            OnUnpublished<TimetableAppointmentPart>((context, bp) => RecalculateTimetableArchive(TimetableArchiveRepository, TimetableAppointmentService, bp));
            OnRemoved<TimetableAppointmentPart>((context, bp) => RecalculateTimetableArchive(TimetableArchiveRepository, TimetableAppointmentService, bp));
        }

        private static void RecalculateTimetableArchive(IRepository<TimetablePartArchiveRecord> TimetableArchiveRepository, ITimetableAppointmentService TimetableAppointmentService, TimetableAppointmentPart TimetableAppointmentPart) {
            TimetableArchiveRepository.Flush();

            // remove all current Timetable archive records
            var TimetableArchiveRecords =
                from bar in TimetableArchiveRepository.Table
                where bar.TimetablePart == TimetableAppointmentPart.TimetablePart.Record
                select bar;
            TimetableArchiveRecords.ToList().ForEach(TimetableArchiveRepository.Delete);

            // get all Timetable appointments for the current Timetable
            var appointments = TimetableAppointmentService.Get(TimetableAppointmentPart.TimetablePart, VersionOptions.Published);

            // create a dictionary of all the year/month combinations and their count of appointments that are published in this Timetable
            var inMemoryTimetableArchives = new Dictionary<DateTime, int>(appointments.Count());
            foreach (var appointment in appointments) {
                if (!appointment.Has<CommonPart>())
                    continue;

                var commonPart = appointment.As<CommonPart>();
                var key = new DateTime(commonPart.PublishedUtc.Value.Year, commonPart.PublishedUtc.Value.Month, 1);

                if (inMemoryTimetableArchives.ContainsKey(key))
                    inMemoryTimetableArchives[key]++;
                else
                    inMemoryTimetableArchives[key] = 1;
            }

            // create the new Timetable archive records based on the in memory values
            foreach (KeyValuePair<DateTime, int> item in inMemoryTimetableArchives) {
                TimetableArchiveRepository.Create(new TimetablePartArchiveRecord {TimetablePart = TimetableAppointmentPart.TimetablePart.Record, Year = item.Key.Year, Month = item.Key.Month, AppointmentCount = item.Value});
            }
        }
    }
}