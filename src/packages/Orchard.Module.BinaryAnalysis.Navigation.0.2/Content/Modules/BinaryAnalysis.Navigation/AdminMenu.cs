using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Core.Navigation;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace BinaryAnalysis.Navigation
{
    [OrchardSuppressDependency("Orchard.Core.Navigation.AdminMenu")]
    public class AdminMenu : INavigationProvider
    {
        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            //todo: - add new menu? and list menus? ...and remove hard-coded menu name here
            builder.AddImageSet("navigation")
                .Add(T("Navigation"), "7",
                    menu => menu
                        .Add(T("Main Menu"), "0", item => item.Action("Index", "Admin", new { area = "BinaryAnalysis.Navigation" })
                        .LocalNav().Permission(Permissions.ManageMainMenu))

                        .Add(T("Hierarchy"), "0", item => item.Action("HierarchyIndex", "Admin", new { area = "BinaryAnalysis.Navigation" })
                        .LocalNav().Permission(Permissions.ManageMainMenu))
                        );
        }
    }
}
