using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.Alias;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Routable.Models;
using Orchard.Routable.Services;
using Orchard.Routable.ViewModels;
using Orchard.Services;
using Orchard.Utility.Extensions;

namespace Orchard.Routable.Drivers {
    public class RoutePartDriver : ContentPartDriver<RoutePart2> {
        private readonly IRoutableService _routableService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoutePartDriver(IRoutableService routableService,
            IHttpContextAccessor httpContextAccessor) {
            _routableService = routableService;
            _httpContextAccessor = httpContextAccessor;
            T = NullLocalizer.Instance;
        }

        private const string TemplateName = "Parts.Routable.RoutePart";

        public Localizer T { get; set; }

        protected override string Prefix {
            get { return "Routable"; }
        }

        protected override DriverResult Display(RoutePart2 part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_RoutableTitle",
                    () => shapeHelper.Parts_RoutableTitle(ContentPart: part, Title: part.Record.Title)),
                ContentShape("Parts_RoutableTitle_Summary",
                    () => shapeHelper.Parts_RoutableTitle_Summary(ContentPart: part, Title: part.Record.Title)),
                ContentShape("Parts_RoutableTitle_SummaryAdmin",
                    () => shapeHelper.Parts_RoutableTitle_SummaryAdmin(ContentPart: part, Title: part.Record.Title))
                );
        }

        protected override DriverResult Editor(RoutePart2 part, dynamic shapeHelper) {
            var request = _httpContextAccessor.Current().Request;
            var model = new RoutableEditorViewModel {
                ContentType = part.ContentItem.ContentType,
                Id = part.ContentItem.Id,
                Title = part.Record.Title,
                Path = _routableService.GetAlias(part.ContentItem),
                BaseUrl = new UriBuilder(request.ToRootUrlString()) { Path = (request.ApplicationPath ?? "").TrimEnd('/') }.Uri.ToString().TrimEnd('/')
            };

            return ContentShape("Parts_Routable_Edit",
                () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: model, Prefix: Prefix));
        }

        protected override DriverResult Editor(RoutePart2 part, IUpdateModel updater, dynamic shapeHelper) {
            var model = new RoutableEditorViewModel();
            updater.TryUpdateModel(model, Prefix, null, null);

            part.Record.Title = model.Title;

            if (!_routableService.IsPathValid(model.Path))
            {
                var slug = (model.Path ?? String.Empty);
                if (slug.StartsWith(".") || slug.EndsWith("."))
                    updater.AddModelError("Routable.Path", T("The \".\" can't be used at either end of the permalink."));
                else
                    updater.AddModelError("Routable.Path", T("Please do not use any of the following characters in your permalink: \":\", \"?\", \"#\", \"[\", \"]\", \"@\", \"!\", \"$\", \"&\", \"'\", \"(\", \")\", \"*\", \"+\", \",\", \";\", \"=\", \", \"<\", \">\", \"\\\". No spaces are allowed (please use dashes or underscores instead)."));
            }
            else
            {
                _routableService.SetAlias(part.ContentItem, model.Path);
            }

            return Editor(part, shapeHelper);
        }

        protected override void Importing(RoutePart2 part, ImportContentContext context) {
            var title = context.Attribute(part.PartDefinition.Name, "Title");
            if (title != null) {
                part.Record.Title = title;
            }

            // todo: export and import the aliases?
            //var path = context.Attribute(part.PartDefinition.Name, "Path");
            //if (path != null) {
            //    part.Path = path;
            //}

        }

        protected override void Exporting(RoutePart2 part, ExportContentContext context) {
            context.Element(part.PartDefinition.Name).SetAttributeValue("Title", part.Record.Title);
            // todo: export and import the aliases?
            //context.Element(part.PartDefinition.Name).SetAttributeValue("Path", part.Path);
            //if (_services.WorkContext.CurrentSite.HomePage == _routableHomePageProvider.GetSettingValue(part.ContentItem.Id)) {
                //context.Element(part.PartDefinition.Name).SetAttributeValue("PromoteToHomePage", "true");   
            //}
        }
    }
}