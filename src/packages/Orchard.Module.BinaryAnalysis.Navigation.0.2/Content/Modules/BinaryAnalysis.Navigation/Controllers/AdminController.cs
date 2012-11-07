using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Orchard;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.Core.Navigation;
using Orchard.Core.Navigation.Models;
using Orchard.Core.Navigation.Services;
using Orchard.Core.Navigation.ViewModels;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.UI;
using Orchard.UI.Navigation;
using Orchard.Utility;

namespace BinaryAnalysis.Navigation.Controllers {
    [ValidateInput(false)]
    public class AdminController : Controller, IUpdateModel {
        private readonly IMenuService _menuService;
        private readonly IOrchardServices _services;
        private readonly INavigationManager _navigationManager;
        private readonly ISignals _signals;

        public AdminController(
            IMenuService menuService,
            IOrchardServices services,
            INavigationManager navigationManager,
            ISignals signals,
            IShapeFactory shapeFactory) {
            _menuService = menuService;
            _services = services;
            _navigationManager = navigationManager;
            _signals = signals;
            T = NullLocalizer.Instance;
            Shape = shapeFactory;
        }

        dynamic Shape { get; set; }
        public Localizer T { get; set; }

        #region IndexActions
        public ActionResult Index(NavigationManagementViewModel model)
        {
            return InternalIndex(model, "Index");
        }
        public ActionResult HierarchyIndex(NavigationManagementViewModel model)
        {
            return InternalIndex(model, "HierarchyIndex");
        }

        private ActionResult InternalIndex(NavigationManagementViewModel model, string actionRedirect)
        {
            if (!_services.Authorizer.Authorize(Permissions.ManageMainMenu, T("Not allowed to manage the main menu")))
                return new HttpUnauthorizedResult();
            if (model == null)
                model = new NavigationManagementViewModel();

            if (model.MenuItemEntries == null || model.MenuItemEntries.Count() < 1)
                model.MenuItemEntries = _menuService.Get().Select(CreateMenuItemEntries).OrderBy(menuPartEntry => menuPartEntry.Position, new FlatPositionComparer()).ToList();

            // need action name as this action is referenced from another action
            return View(actionRedirect, model);
        } 
        #endregion

        #region IndexUpdate actions
        [HttpPost, ActionName("Index")]
        public ActionResult IndexPOST(IList<MenuItemEntry> menuItemEntries)
        {
            return InternalIndexPOST(menuItemEntries, "Index");
        }
        [HttpPost, ActionName("HierarchyIndex")]
        public ActionResult HierarchyPOST(IList<MenuItemEntry> menuItemEntries)
        {
            return InternalIndexPOST(menuItemEntries, "HierarchyIndex");
        }

        private ActionResult InternalIndexPOST(IList<MenuItemEntry> menuItemEntries, string actionRedirect)
        {
            if (!_services.Authorizer.Authorize(Permissions.ManageMainMenu, T("Couldn't manage the main menu")))
                return new HttpUnauthorizedResult();

            // See http://orchard.codeplex.com/workitem/17116
            if (menuItemEntries != null)
            {
                foreach (var menuItemEntry in menuItemEntries)
                {
                    MenuPart menuPart = _menuService.Get(menuItemEntry.MenuItemId);

                    menuPart.MenuText = menuItemEntry.Text;
                    menuPart.MenuPosition = menuItemEntry.Position;
                    if (menuPart.Is<MenuItemPart>())
                        menuPart.As<MenuItemPart>().Url = menuItemEntry.Url;
                }

                _signals.Trigger("MenuPart.Updated");
            }
            return RedirectToAction(actionRedirect);
        } 
        #endregion

        #region Create
        private MenuItemEntry CreateMenuItemEntries(MenuPart menuPart)
        {
            return new MenuItemEntry
            {
                MenuItemId = menuPart.Id,
                IsMenuItem = menuPart.Is<MenuItemPart>(),
                Text = menuPart.MenuText,
                Position = menuPart.MenuPosition,
                Url = menuPart.Is<MenuItemPart>()
                              ? menuPart.As<MenuItemPart>().Url
                              : _navigationManager.GetUrl(null, _services.ContentManager.GetItemMetadata(menuPart).DisplayRouteValues),
            };
        }

        public ActionResult Create()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(NavigationManagementViewModel model)
        {
            return InternalCreate(model, "Index");
        }
        [HttpPost]
        public ActionResult HierarchyCreate(NavigationManagementViewModel model)
        {
            return InternalCreate(model, "HierarchyIndex");
        }

        private ActionResult InternalCreate(NavigationManagementViewModel model, string actionRedirect)
        {
            if (!_services.Authorizer.Authorize(Permissions.ManageMainMenu, T("Couldn't manage the main menu")))
                return new HttpUnauthorizedResult();

            var menuPart = _services.ContentManager.New<MenuPart>("MenuItem");
            menuPart.OnMainMenu = true;
            menuPart.MenuText = model.NewMenuItem.Text;
            menuPart.MenuPosition = model.NewMenuItem.Position;
            if (string.IsNullOrEmpty(menuPart.MenuPosition))
                menuPart.MenuPosition = Position.GetNext(_navigationManager.BuildMenu("main"));

            var menuItem = menuPart.As<MenuItemPart>();
            menuItem.Url = model.NewMenuItem.Url;

            if (!ModelState.IsValid)
            {
                _services.TransactionManager.Cancel();
                return View(actionRedirect, model);
            }

            _services.ContentManager.Create(menuPart);
            _signals.Trigger("MenuPart.Updated");

            return RedirectToAction(actionRedirect);
        } 
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            return InternalDelete(id, "Index");
        }
        [HttpPost]
        public ActionResult HierarchyDelete(int id)
        {
            return InternalDelete(id, "HierarchyIndex");
        }

        private ActionResult InternalDelete(int id, string actionRedirect)
        {
            if (!_services.Authorizer.Authorize(Permissions.ManageMainMenu, T("Couldn't manage the main menu")))
                return new HttpUnauthorizedResult();

            MenuPart menuPart = _menuService.Get(id);

            if (menuPart != null)
            {
                if (menuPart.Is<MenuItemPart>())
                    _menuService.Delete(menuPart);
                else
                    menuPart.OnMainMenu = false;
            }

            _signals.Trigger("MenuPart.Updated");
            return RedirectToAction(actionRedirect);
        } 
        #endregion

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}
