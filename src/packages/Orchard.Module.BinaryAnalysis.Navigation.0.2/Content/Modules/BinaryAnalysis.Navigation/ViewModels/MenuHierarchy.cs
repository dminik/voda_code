using System.Collections.Generic;
using System.Linq;
using Orchard.UI.Navigation;

namespace BinaryAnalysis.Navigation.ViewModels
{
    public class MenuHierarchy {
        public MenuHierarchy() : this(null) {}

        public MenuHierarchy(MenuItem item)
        {
            Item = item;
            Children = new List<MenuHierarchy>();
        }
        public int Order { get; set; }
        public int Level { get; set; }
        public MenuItem Item { get; set; }
        public List<MenuHierarchy> Children { get; set; }

        public bool HasSelected()
        {
            return Item != null && Item.Selected || Children.Any(m => m.HasSelected());
        }
        public MenuHierarchy GetSelectedTree() {
            return Children.FirstOrDefault(m => m.HasSelected());
        }

    }
}
