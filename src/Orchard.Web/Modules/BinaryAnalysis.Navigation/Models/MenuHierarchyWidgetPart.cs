using Orchard.ContentManagement;

namespace BinaryAnalysis.Navigation.Models
{
    /// <summary>
    /// Part for displaying the menu.
    /// </summary>
    public class MenuHierarchyWidgetPart : ContentPart<MenuHierarchyWidgetPartRecord>
    {
        public int LevelToStart
        {
            get { return Record.LevelToStart; }
            set { Record.LevelToStart = value; }
        }
        public int LevelsToShow
        {
            get { return Record.LevelsToShow; }
            set { Record.LevelsToShow = value; }
        }
    }
}