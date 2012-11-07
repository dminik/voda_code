using System.Collections.Generic;
using BinaryAnalysis.Navigation.Models;
using BinaryAnalysis.Navigation.Services;
using BinaryAnalysis.Navigation.ViewModels;
using JetBrains.Annotations;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;

namespace BinaryAnalysis.Navigation.Drivers
{
    [UsedImplicitly]
    public class MenuHierarchyWidgetPartDriver : ContentPartDriver<MenuHierarchyWidgetPart>
    {
        private readonly IOrchardServices Services;
        private readonly IMenuHierarchyService _menuHierarchyService;
        private readonly INotifier _notifier;

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public MenuHierarchyWidgetPartDriver(
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

        protected override DriverResult Display(MenuHierarchyWidgetPart part, string displayType, dynamic shapeHelper)
        {
            var hierarchy = _menuHierarchyService.GetHierachialModelWithLimits(part.LevelToStart, part.LevelsToShow);
            MenuItem rootItem = null;
            if (hierarchy != null) {//hide parent for submenu
                rootItem = hierarchy.Item;
                hierarchy.Item = null; 
            }

            return ContentShape("Parts_MenuHierarchyWidget", 
                () => shapeHelper.Parts_MenuHierarchyWidget(ContentPart: part, RootItem: rootItem, Menu: hierarchy));
        }

        protected override DriverResult Editor(MenuHierarchyWidgetPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_MenuHierarchyWidget",
                    () => shapeHelper.EditorTemplate(TemplateName: "Parts.MenuHierarchyWidget", 
                        Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(MenuHierarchyWidgetPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
                _notifier.Information(T("MenuHierarchy edited successfully"));
            }
            else
            {
                _notifier.Error(T("Error during MenuHierarchy update!"));
            }
            return Editor(part, shapeHelper);
        }
    }
}
