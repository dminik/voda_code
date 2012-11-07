using Orchard.ContentManagement.Records;

namespace BinaryAnalysis.Navigation.Models {
    public class MenuHierarchyWidgetPartRecord : ContentPartRecord 
    {
        public virtual int LevelsToShow { get; set; }
        public virtual int LevelToStart { get; set; }
    }
}