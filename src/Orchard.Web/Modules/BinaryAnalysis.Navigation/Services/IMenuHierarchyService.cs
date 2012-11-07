using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryAnalysis.Navigation.Models;
using BinaryAnalysis.Navigation.ViewModels;
using Orchard;
using Orchard.UI.Navigation;

namespace BinaryAnalysis.Navigation.Services
{
    public interface IMenuHierarchyService : IDependency {
        MenuHierarchy GetHierachialModel();
        MenuHierarchy GetHierachialModelWithLimits(int levelStart, int levelEnd);
        MenuItem GetSelectedMenuItem();
        IEnumerable<MenuItem> PathToSelected();
    }
}
