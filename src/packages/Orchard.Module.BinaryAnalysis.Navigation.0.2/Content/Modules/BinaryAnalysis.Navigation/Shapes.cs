using BinaryAnalysis.Navigation.Models;
using BinaryAnalysis.Navigation.ViewModels;
using Orchard;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment;
using Orchard.UI.Navigation;
using Orchard.Utility.Extensions;

namespace BinaryAnalysis.Navigation
{
    public class Shapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly Work<INavigationManager> _navigationManager;

        public Shapes(
            Work<WorkContext> workContext,
            Work<INavigationManager> navigationManager)
        {
            _workContext = workContext;
            _navigationManager = navigationManager;
        }

        #region Implementation of IShapeTableProvider
        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("Parts_MenuHierarchyWidget")
                .OnDisplaying(
                    displaying => {
                        var ci = displaying.Shape.ContentPart as MenuHierarchyWidgetPart;
                        var rootItem = displaying.Shape.RootItem as MenuItem;
                        if (ci != null) {
                            displaying.ShapeMetadata.Alternates.Add(
                                displaying.ShapeMetadata.Type + string.Format("-Level-{0}-{1}", ci.LevelToStart, ci.LevelsToShow));
                        }
                        if (rootItem != null) {
                            displaying.ShapeMetadata.Alternates.Add(
                                displaying.ShapeMetadata.Type + string.Format("-{0}", rootItem.Text.Text.HtmlClassify()));
                        }
                    });
        }
        #endregion
    }
}
