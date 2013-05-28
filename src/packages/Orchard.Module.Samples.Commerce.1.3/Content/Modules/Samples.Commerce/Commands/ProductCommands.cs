namespace Samples.Commerce.Commands
{
    using System;
    using Models;
    using Orchard.Commands;
    using Orchard.ContentManagement;
    using Orchard.ContentManagement.Aspects;
    using Orchard.Core.Common.Models;
    using Orchard.Core.Routable.Models;
    using Orchard.Security;
    using Orchard.Settings;

    public class ProductCommands : DefaultOrchardCommandHandler
    {
        private readonly IContentManager contentManager;
        private readonly IMembershipService membershipService;
        private readonly ISiteService siteService;

        public ProductCommands(IContentManager contentManager, IMembershipService membershipService, ISiteService siteService)
        {
            this.contentManager = contentManager;
            this.membershipService = membershipService;
            this.siteService = siteService;
        }

        [OrchardSwitch]
        public string Title { get; set; }

        [OrchardSwitch]
        public string Description { get; set; }

        [OrchardSwitch]
        public decimal Price { get; set; }

        [OrchardSwitch]
        public string Sku { get; set; }

        [OrchardSwitch]
        public string ImageUrl { get; set; }

        [OrchardSwitch]
        public string Owner { get; set; }

        [CommandName("product create")]
        [CommandHelp("product create /Title:<title> /Description:<description> /Price:<price> /Sku:<sku> /ImageUrl:<imageurl> [/Owner:<username>]\r\n\t" + "Creates a new product")]
        [OrchardSwitches("Title,Description,Price,Sku,Owner,ImageUrl")]
        public void Create()
        {
            if (String.IsNullOrEmpty(this.Owner))
            {
                this.Owner = this.siteService.GetSiteSettings().SuperUser;
            }

            var owner = this.membershipService.GetUser(this.Owner);

            var product = this.contentManager.Create("Product");
            product.As<ProductPart>().Description = this.Description;
            product.As<ProductPart>().ImageUrl = this.ImageUrl;
            product.As<ProductPart>().Price = this.Price;
            product.As<ProductPart>().Sku = this.Sku;
            product.As<RoutePart>().Title = this.Title;

            product.As<ICommonPart>().Owner = owner;

            this.contentManager.Publish(product);

            Context.Output.WriteLine(T("Product created successfully.").Text);
        }

        [CommandName("product createlist")]
        [CommandHelp("product createlist /Title:<title>\r\n\t" + "Creates a new product list")]
        [OrchardSwitches("Title")]
        public void CreateProductList()
        {
            if (String.IsNullOrEmpty(this.Owner))
            {
                this.Owner = this.siteService.GetSiteSettings().SuperUser;
            }

            var owner = this.membershipService.GetUser(this.Owner);

            var list = this.contentManager.Create("List");
            list.As<ICommonPart>().Owner = owner;

            // iterate products
            list.As<RoutePart>().Title = "ProductList";

            var item = this.contentManager.Create("Product");
            item.As<CommonPart>().Record.Container = list.Record;
        }

        [CommandName("product createsample")]
        [CommandHelp("product createsample /Title:<title>\r\n\t" + "Creates a new product list sample")]
        [OrchardSwitches("Title")]
        public void CreateSample()
        {
            if (String.IsNullOrEmpty(this.Owner))
            {
                this.Owner = this.siteService.GetSiteSettings().SuperUser;
            }

            var owner = this.membershipService.GetUser(this.Owner);

            var list = this.contentManager.New("List");
            list.As<ICommonPart>().Owner = owner;
            list.As<RoutePart>().Title = this.Title;
            list.As<RoutePart>().Slug = "product-list";
            list.As<RoutePart>().Path = "product-list";

            this.contentManager.Create(list);
            this.contentManager.Publish(list);

            ContentItem product = this.CreateProduct("XBox Controller", "Joystick Pad for XBox", "77", (decimal)235.99d, "http://images.wikia.com/es.gta/images/4/48/Xbox-control.jpg", "xbox-controller");
            product.As<CommonPart>().Record.Container = list.Record;

            this.contentManager.Publish(product);

            product = this.CreateProduct("Marvel vs Capcom 3", "Marvel vs Capcom 3 Fate of Two worlds", "88", (decimal)77.99d, "http://novedadesxbox.com/wp-content/uploads/2011/03/portada-del-marvel-vs-capcom-3-para-xbox-360.jpg", "marvel-vs-capcom");
            product.As<CommonPart>().Record.Container = list.Record;

            this.contentManager.Publish(product);
        }

        private ContentItem CreateProduct(string title, string description, string sku, decimal price, string imageUrl, string slug)
        {
            if (String.IsNullOrEmpty(this.Owner))
            {
                this.Owner = this.siteService.GetSiteSettings().SuperUser;
            }

            var owner = this.membershipService.GetUser(this.Owner);

            var product = this.contentManager.New("Product");
            product.As<ProductPart>().Description = description;
            product.As<ProductPart>().ImageUrl = imageUrl;
            product.As<ProductPart>().Price = price;
            product.As<ProductPart>().Sku = sku;
            product.As<RoutePart>().Title = title;
            product.As<RoutePart>().Slug = slug;
            product.As<RoutePart>().Path = slug;

            product.As<ICommonPart>().Owner = owner;

            this.contentManager.Create(product);

            return product;
        }
    }
}