using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.SEO.Models;


namespace Orchard.SEO.Handlers
{
    public class MetaHandler : ContentHandler
    {
        public MetaHandler(IRepository<MetaRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}