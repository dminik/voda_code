using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryAnalysis.Navigation.Models;
using BinaryAnalysis.Navigation.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.UI.Notify;

namespace BinaryAnalysis.Navigation.Drivers
{
    public class MenuBreadcrumbWidgetPartDriver : ContentPartDriver<MenuBreadcrumbWidgetPart>
    {
        private readonly IOrchardServices Services;
        private readonly IMenuHierarchyService _menuHierarchyService;
        private readonly INotifier _notifier;

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public MenuBreadcrumbWidgetPartDriver(
            IOrchardServices services,
            IMenuHierarchyService menuHierarchyService,
            INotifier notifier
            ) {
            Services = services;
            _menuHierarchyService = menuHierarchyService;
            _notifier = notifier;

            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        protected override DriverResult Display(MenuBreadcrumbWidgetPart part, string displayType, dynamic shapeHelper) {
            var menuItems = _menuHierarchyService.PathToSelected();
            return ContentShape("Parts_MenuBreadcrumbWidget",
                () => shapeHelper.Parts_MenuBreadcrumbWidget(ContentPart: part, Breadcrumb: menuItems));
        }

        protected override DriverResult Editor(MenuBreadcrumbWidgetPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_MenuBreadcrumbWidget",
                    () => shapeHelper.EditorTemplate(TemplateName: "Parts.MenuBreadcrumbWidget",
                        Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(MenuBreadcrumbWidgetPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
                _notifier.Information(T("MenuBreadcrumb edited successfully"));
            }
            else
            {
                _notifier.Error(T("Error during MenuBreadcrumb update!"));
            }
            return Editor(part, shapeHelper);
        }
    }
}
