namespace Samples.Commerce.Models
{
    using System.ComponentModel.DataAnnotations;

    using Orchard.ContentManagement;

    public class ProductPart : ContentPart<ProductPartRecord>
    {
        public string Sku
        {
            get { return Record.Sku; }
            set { Record.Sku = value; }
        }

        [Required]
        public decimal Price
        {
            get { return Record.Price; }
            set { Record.Price = value; }
        }

        public string ImageUrl
        {
            get { return Record.ImageUrl; }
            set { Record.ImageUrl = value; }
        }

        public string Description
        {
            get { return Record.Description; }
            set { Record.Description = value; }
        }
    }
}