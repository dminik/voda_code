using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.SEO.Models;


namespace Orchard.SEO {
	public class Migrations : DataMigrationImpl {

		public int Create() {
			// Creating table MetaRecord
			SchemaBuilder.CreateTable("MetaRecord", table => table
				.ContentPartRecord()
				.Column("Keywords", DbType.String)
				.Column("Description", DbType.String)
			);

			ContentDefinitionManager.AlterPartDefinition(
				typeof(MetaPart).Name, cfg => cfg.Attachable());

			return 1;
		}
	}
}