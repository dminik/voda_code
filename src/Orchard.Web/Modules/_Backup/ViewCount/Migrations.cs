using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using ViewCount.Models;

namespace Orchard.ViewCount
{
    public class Migrations : DataMigrationImpl
    {

        public int Create()
        {
            // Creating table ViewCountRecord
            SchemaBuilder.CreateTable("ViewCountRecord", table => table
                .ContentPartRecord()
                .Column("ViewCount", DbType.Int32)
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(ViewCountPart).Name, cfg => cfg.Attachable());

            return 1;
        }
    }
}