using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Definition;
using Orchard.ContentManagement.Drivers;
using Orchard.Data.Migration;
using Orchard.ContentManagement.Definition;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Orchard.Routable {
    public class Migrations : DataMigrationImpl {

        public int Create() {
			// Creating table RoutePart2Record
			SchemaBuilder.CreateTable("RoutePart2Record", table => table
				.ContentPartVersionRecord()
				.Column("Title", DbType.String, column => column.WithLength(1024))
			);

            ContentDefinitionManager.AlterPartDefinition("RoutePart2", builder => builder.Attachable());


            return 1;
        }
    }
}