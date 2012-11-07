using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Orchard.SEO.Models;
using Orchard.UI.Resources;
using Orchard.ContentManagement;


namespace Orchard.SEO.Drivers
{
    public class MetaDriver : ContentPartDriver<MetaPart>
    {
        private readonly IWorkContextAccessor _wca;

        public MetaDriver(IWorkContextAccessor workContextAccessor)
        {
            _wca = workContextAccessor;
        }

        protected override string Prefix
        {
            get
            {
                return "meta";
            }
        }

        protected override DriverResult Display(MetaPart part, string displayType, dynamic shapeHelper)
        {
            if (displayType != "Detail") return null;
            var resourceManager = _wca.GetContext().Resolve<IResourceManager>();
            if (!String.IsNullOrWhiteSpace(part.Description))
            {
                resourceManager.SetMeta(new MetaEntry
                {
                    Name = "description",
                    Content = part.Description
                });
            }
            if (!String.IsNullOrWhiteSpace(part.Keywords))
            {
                resourceManager.SetMeta(new MetaEntry
                {
                    Name = "keywords",
                    Content = part.Keywords
                });
            }
            return null;
        }

        //GET
        protected override DriverResult Editor(MetaPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_Meta_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/Meta",
                    Model: part,
                    Prefix: Prefix));
        }
        //POST
        protected override DriverResult Editor(MetaPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}