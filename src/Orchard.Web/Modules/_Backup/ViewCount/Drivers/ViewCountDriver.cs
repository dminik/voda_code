using System;
using ViewCount.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace ViewCount.Drivers
{
    public class ViewCountDriver : ContentPartDriver<ViewCountPart>
    {
        protected override DriverResult Display(ViewCountPart part, string displayType, dynamic shapeHelper)
        {
            part.ViewCount++;

            return ContentShape("Parts_ViewCount",
                () => shapeHelper.Parts_ViewCount(
                    ViewCount: part.ViewCount
            ));
        }

        //GET
        protected override DriverResult Editor(ViewCountPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_ViewCount_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/ViewCount",
                    Model: part,
                    Prefix: Prefix
            ));
        }
        //POST
        protected override DriverResult Editor(ViewCountPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }

}