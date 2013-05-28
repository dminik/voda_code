namespace Samples.Commerce
{
    using System.Collections.Generic;
    using System.Data;

    using Models;

    using Orchard.ContentManagement.Drivers;
    using Orchard.ContentManagement.MetaData;
    using Orchard.ContentManagement.MetaData.Builders;
    using Orchard.Core.Contents.Extensions;
    using Orchard.Data.Migration;
    using Orchard.Indexing;

    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(
                    typeof(ProductPartRecord).Name,
                    table => table.ContentPartRecord()
                                  .Column("Sku", DbType.String)
                                  .Column("Price", DbType.Currency)
                                  .Column("ImageUrl", DbType.String)
                                  .Column("Description", DbType.String));

            ContentDefinitionManager.AlterPartDefinition(
                typeof(ProductPart).Name,
                builder => builder.Attachable()
                                  .WithLocation(
                                                new Dictionary<string, ContentLocation> 
                                                {
                                                    { "Default", new ContentLocation { Zone = "primary", Position = "3" } },
                                                    { "Editor", new ContentLocation { Zone = "primary", Position = "3" } } 
                                                }));
            
            ContentDefinitionManager.AlterTypeDefinition(
                "Product",
                cfg => cfg.WithPart("CommonPart")
                          .WithPart("RoutePart")
                          .WithPart("ProductPart")
                          .WithPart("LocalizationPart")
                          .WithPart("ContainablePart")
                          .Creatable()
                          .Indexed());

            return 1;
        }
    }
}

