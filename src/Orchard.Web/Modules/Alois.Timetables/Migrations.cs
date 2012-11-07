using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace Alois.Timetables {
    public class Migrations : DataMigrationImpl {

        public int Create() {
            SchemaBuilder.CreateTable("TimetablePartArchiveRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<int>("Year")
                    .Column<int>("Month")
                    .Column<int>("AppointmentCount")
                    .Column<int>("TimetablePart_id")
                );

            SchemaBuilder.CreateTable("TimetablePartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("Description", c => c.Unlimited())
                    .Column<int>("AppointmentCount")
                );

            SchemaBuilder.CreateTable("RecentTimetableAppointmentsPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("TimetableSlug")
                    .Column<int>("Count")
                );

            SchemaBuilder.CreateTable("TimetableArchivesPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("TimetableSlug", c => c.WithLength(255))
                );

            ContentDefinitionManager.AlterTypeDefinition("Timetable",
                cfg => cfg
                    .WithPart("TimetablePart")
                    .WithPart("CommonPart")
                    .WithPart("RoutePart")
                );

            ContentDefinitionManager.AlterTypeDefinition("TimetableAppointment",
                cfg => cfg
                    .WithPart("TimetableAppointmentPart")
                    .WithPart("CommonPart")
                    .WithPart("EventDatePart")
                );
            
            ContentDefinitionManager.AlterTypeDefinition("RecentTimetableAppointments",
                cfg => cfg
                    .WithPart("RecentTimetableAppointmentsPart")
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );

            ContentDefinitionManager.AlterTypeDefinition("TimetableArchives",
                cfg => cfg
                    .WithPart("TimetableArchivesPart")
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );

            return 1;
        }
    }
}