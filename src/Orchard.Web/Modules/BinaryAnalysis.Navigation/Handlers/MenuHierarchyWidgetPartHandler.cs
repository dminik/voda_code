using BinaryAnalysis.Navigation.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace BinaryAnalysis.Navigation.Handlers
{
    [UsedImplicitly]
    public class MenuHierarchyWidgetPartHandler : ContentHandler
    {
        public MenuHierarchyWidgetPartHandler(IRepository<MenuHierarchyWidgetPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));

        }
    }
}
