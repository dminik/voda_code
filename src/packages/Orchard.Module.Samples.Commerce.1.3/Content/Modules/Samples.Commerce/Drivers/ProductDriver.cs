namespace Samples.Commerce.Drivers
{
    using System;
    using Models;
    using Orchard.ContentManagement;
    using Orchard.ContentManagement.Drivers;
    using Orchard.ContentManagement.Handlers;

    public class ProductDriver : ContentPartDriver<ProductPart>
    {
        protected override DriverResult Display(ProductPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape(
                    "Parts_Product",
                    () => shapeHelper.Parts_Product(Sku: part.Sku, Price: part.Price, ImageUrl: part.ImageUrl, Description: part.Description));
        }

        protected override DriverResult Editor(ProductPart part, dynamic shapeHelper)
        {
            return ContentShape(
                    "Parts_Product_Edit",
                    () => shapeHelper.EditorTemplate(TemplateName: "Parts/Product", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(ProductPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return this.Editor(part, shapeHelper);
        }

        protected override void Importing(ProductPart part, ImportContentContext context)
        {
            var price = context.Attribute(part.PartDefinition.Name, "Price");

            if (price != null)
                part.Price = Convert.ToDecimal(price);

            var sku = context.Attribute(part.PartDefinition.Name, "Sku");

            if (sku != null)
                part.Sku = sku;

            var description = context.Attribute(part.PartDefinition.Name, "Description");

            if (description != null)
                part.Description = description;

            var imageurl = context.Attribute(part.PartDefinition.Name, "ImageUrl");

            if (imageurl != null)
                part.ImageUrl = imageurl;
        }

        protected override void Exporting(ProductPart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("Price", part.Price);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Sku", part.Sku);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Description", part.Description);
            context.Element(part.PartDefinition.Name).SetAttributeValue("ImageUrl", part.ImageUrl);
        }
    }
}