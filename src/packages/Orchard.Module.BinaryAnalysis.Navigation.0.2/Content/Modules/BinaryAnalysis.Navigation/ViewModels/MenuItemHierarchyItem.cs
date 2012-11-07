using System.Collections.Generic;
using System.Linq;
using Orchard.Core.Navigation.ViewModels;

namespace BinaryAnalysis.Navigation.ViewModels
{
    public class MenuItemHierarchyItem {
        public const string SEPARATOR = ".";

        public static MenuItemHierarchyItem CreateFromModel(NavigationManagementViewModel model) {
            var usedItems = new List<MenuItemEntry>();
            var result = new MenuItemHierarchyItem(null);
            //create root elements
            foreach (var item in model.MenuItemEntries)
            {
                if (!item.Position.Contains(SEPARATOR)) {
                    result.Children.Add(new MenuItemHierarchyItem(item));
                    usedItems.Add(item);
                }
            }
            foreach (var item in result.Children) {
                var childUsedItems = CreateChildrenFromModel(item, model);
                usedItems.AddRange(childUsedItems);
            }
            
            foreach (var item in model.MenuItemEntries.Except(usedItems).ToList()) {
                result.Children.Add(new MenuItemHierarchyItem(item));
                usedItems.Add(item);
            }

            //set order
            SetOrder(result.Children, 0);

            return result;
        }

        private static int SetOrder(List<MenuItemHierarchyItem> children, int i) {
            foreach (var menuItemHierarchyItem in children) {
                menuItemHierarchyItem.Order = i++;
                i = SetOrder(menuItemHierarchyItem.Children, i);
            }
            return i;
        }

        private static List<MenuItemEntry> CreateChildrenFromModel(MenuItemHierarchyItem hitem, NavigationManagementViewModel model)
        {
            var usedItems = new List<MenuItemEntry>();

            var startStr = hitem.Item.Position + SEPARATOR;
            foreach (var item in model.MenuItemEntries) 
            {
                if (item.Position.StartsWith(startStr) && item.Position.LastIndexOf(SEPARATOR) == startStr.Length - 1) {
                    hitem.Children.Add(new MenuItemHierarchyItem(item));
                    usedItems.Add(item);
                }
            }
            foreach (var item in hitem.Children) usedItems.AddRange(CreateChildrenFromModel(item, model));
            return usedItems;
        }

        public MenuItemHierarchyItem(MenuItemEntry item) {
            Item = item;
            Children = new List<MenuItemHierarchyItem>();
        }

        public string Separator { get { return SEPARATOR;  } }

        public int Order { get; set; }
        public MenuItemEntry Item { get; set; }
        public List<MenuItemHierarchyItem> Children { get; set; }
    }
}
