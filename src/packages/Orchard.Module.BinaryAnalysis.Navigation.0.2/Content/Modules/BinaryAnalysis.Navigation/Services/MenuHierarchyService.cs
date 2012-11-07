using System;
using System.Collections.Generic;
using System.Linq;
using BinaryAnalysis.Commons.Services;
using BinaryAnalysis.Navigation.Models;
using BinaryAnalysis.Navigation.ViewModels;
using Orchard;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.UI.Navigation;

namespace BinaryAnalysis.Navigation.Services {
    public class MenuHierarchyService : IMenuHierarchyService 
    {
        public const string SEPARATOR = ".";

        public readonly IOrchardServices Services;
        public ILogger Logger { get; set; }
        public Localizer T { get; set; }
        private readonly INavigationManager _navigationManager;
        private readonly IClassMappingService _classMappingService;

        public MenuHierarchyService(IOrchardServices orchardServices,
            INavigationManager navigationManager,
            IClassMappingService classMappingService
            )
        {
            Services = orchardServices;
            _navigationManager = navigationManager;
            _classMappingService = classMappingService;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        #region Implementation of IMenuHierarchyService

        public MenuItem GetSelectedMenuItem() {
            foreach (var menuItem in GetBuildMenu())
            {
                var httpContext = Services.WorkContext.HttpContext;
                string requestUrl = httpContext.Request.Path.Replace(httpContext.Request.ApplicationPath, string.Empty).TrimEnd('/').ToUpperInvariant();
                string modelUrl = menuItem.Href.Replace(httpContext.Request.ApplicationPath, string.Empty).TrimEnd('/').ToUpperInvariant();
                if ((!string.IsNullOrEmpty(modelUrl) && requestUrl.StartsWith(modelUrl)) || requestUrl == modelUrl)
                {
                    return menuItem;
                }
            }
            return null;
        }

        public MenuHierarchy GetHierachialModelWithLimits(int levelStart, int levelEnd) 
        {
            var hierarchy = GetHierachialModel();
            
            var clevelStart = levelStart;
            while (hierarchy != null && clevelStart-- > 0) hierarchy = hierarchy.GetSelectedTree();
            CutLevel(hierarchy.Children, levelEnd + levelStart);
            return hierarchy;
        }


        public IEnumerable<MenuItem> PathToSelected()
        {
            var hierarchy = GetHierachialModel();
            var result = new List<MenuItem>();
            while (hierarchy!=null) {
                if (hierarchy.Item != null) result.Add(hierarchy.Item);
                hierarchy = hierarchy.GetSelectedTree();
            }
            return result;
        }

        private MenuHierarchy getHierachialModel;
        private MenuHierarchy CloneModel(MenuHierarchy hierachialModel) {
            var mh = _classMappingService.MapObject<MenuHierarchy>(hierachialModel);
            mh.Children = new List<MenuHierarchy>();
            mh.Children.AddRange(hierachialModel.Children.Select(CloneModel));
            return mh;
        }
        public MenuHierarchy GetHierachialModel() {

            if (getHierachialModel != null) return CloneModel(getHierachialModel);
            var list = GetBuildMenu();
            var usedItems = new List<MenuItem>();
            getHierachialModel = new MenuHierarchy(null);

            var selectedItem = GetSelectedMenuItem();
            //create root elements
            foreach (var item in list)
            {
                if (selectedItem.Position == item.Position) item.Selected = true;
                //if (selectedItem.Href == item.Href) item.Selected = true;
                if (!item.Position.Contains(SEPARATOR))
                {
                    getHierachialModel.Children.Add(new MenuHierarchy(item));
                    usedItems.Add(item);
                }
            }
            foreach (var item in getHierachialModel.Children)
            {
                var childUsedItems = CreateChildren(item, list);
                usedItems.AddRange(childUsedItems);
            }

            foreach (var item in list.Except(usedItems).ToList())
            {
                getHierachialModel.Children.Add(new MenuHierarchy(item));
                usedItems.Add(item);
            }

            //set order
            SetOrder(getHierachialModel.Children, 0);

            return CloneModel(getHierachialModel);
        }

        #endregion

        #region Private helpers

        private IEnumerable<MenuItem> GetBuildMenu() 
        {
            string menuName = "main";
            var list = _navigationManager.BuildMenu(menuName);
            return list;
        }


        private static int SetOrder(List<MenuHierarchy> children, int i)
        {
            foreach (var menuItemHierarchyItem in children)
            {
                menuItemHierarchyItem.Order = i++;
                i = SetOrder(menuItemHierarchyItem.Children, i);
            }
            return i;
        }
        private static void CutLevel(List<MenuHierarchy> children, int level)
        {
            if(level<=0) return;
            foreach (var menuItemHierarchyItem in children)
            {
                if (menuItemHierarchyItem.Level==level-1) menuItemHierarchyItem.Children.Clear();
                CutLevel(menuItemHierarchyItem.Children, level);
            }
        }

        private static IEnumerable<MenuItem> CreateChildren(MenuHierarchy hitem, IEnumerable<MenuItem> list)
        {
            var usedItems = new List<MenuItem>();

            var startStr = hitem.Item.Position + SEPARATOR;
            foreach (var item in list)
            {
                if (item.Position.StartsWith(startStr) && item.Position.LastIndexOf(SEPARATOR) == startStr.Length - 1)
                {
                    hitem.Children.Add(new MenuHierarchy(item) { Level = hitem.Level+1});
                    usedItems.Add(item);
                }
            }
            foreach (var item in hitem.Children) usedItems.AddRange(CreateChildren(item, list));
            return usedItems;
        }
        #endregion
    }
}