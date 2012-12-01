using Orchard.Localization;
using Orchard.UI.Navigation;

namespace Orchard.ContentTypes {
    public class AdminMenu : INavigationProvider {

        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T("Content"),
                        menu => menu
                                    .Add(T("Content Types"), "3", item => item.Action("Index", "Admin", new {area = "Orchard.ContentTypes"}).LocalNav())                                    
									.Add(T("Content Parts"), "5", item => item.Action("ListParts", "Admin", new { area = "Orchard.ContentTypes" }).LocalNav()));

			builder.AddImageSet("Content Parts")
				.Add(this.T("Content Parts"), "1",
					menu => menu
									.Add(T("Content Parts"), "1", item => item.Action("ListParts", "Admin", new { area = "Orchard.ContentTypes" }).LocalNav()));

			builder.AddImageSet("Content Types")
				.Add(this.T("Content Types"), "1",
					menu => menu
									.Add(T("Content Type"), "1", item => item.Action("Index", "Admin", new { area = "Orchard.ContentTypes" }).LocalNav()));
        }
    }
}