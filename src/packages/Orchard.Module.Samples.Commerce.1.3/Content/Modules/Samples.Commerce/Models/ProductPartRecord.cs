namespace Samples.Commerce.Models
{
    using Orchard.ContentManagement.Records;

    public class ProductPartRecord : ContentPartRecord
    {
        public virtual string Sku { get; set; }

        public virtual decimal Price { get; set; }

        public virtual string ImageUrl { get; set; }

        public virtual string Description { get; set; }
    }
}