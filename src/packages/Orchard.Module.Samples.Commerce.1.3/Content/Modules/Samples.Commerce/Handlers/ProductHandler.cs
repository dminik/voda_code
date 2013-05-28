namespace Samples.Commerce.Handlers
{
    using Models;

    using Orchard.ContentManagement.Handlers;
    using Orchard.Data;

    public class ProductHandler : ContentHandler
    {
        public ProductHandler(IRepository<ProductPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}