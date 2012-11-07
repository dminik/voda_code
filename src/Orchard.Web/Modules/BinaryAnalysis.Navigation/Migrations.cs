using BinaryAnalysis.Navigation.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace BinaryAnalysis.Navigation
{
    public class Migrations : DataMigrationImpl
    {
        public int Create() 
        {
            SchemaBuilder.CreateTable(typeof(MenuHierarchyWidgetPartRecord).Name, table => table
                .ContentPartRecord()
                .Column<int>("LevelsToShow")
                .Column<int>("LevelToStart")
            );

            ContentDefinitionManager.AlterPartDefinition(typeof(MenuHierarchyWidgetPart).Name,
                builder => builder.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("MenuHierarchyWidget",
                cfg => cfg
                    .WithPart(typeof(MenuHierarchyWidgetPart).Name)
                    .WithPart("WidgetPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget"));

            return 1;
        }

        public int UpdateFrom1()
        {

            ContentDefinitionManager.AlterPartDefinition(typeof(MenuBreadcrumbWidgetPart).Name,
                builder => builder.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("MenuBreadcrumbWidget",
                cfg => cfg
                    .WithPart(typeof(MenuBreadcrumbWidgetPart).Name)
                    .WithPart("WidgetPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget"));
            
            return 2;
        }
    }
}
