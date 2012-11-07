using System;
using ViewCount.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace ViewCount.Handlers
{
    public class ViewCountHandler : ContentHandler
    {
        public ViewCountHandler(IRepository<ViewCountRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}