using System.ComponentModel.DataAnnotations;

namespace Orchard.Routable.ViewModels {
    public class RoutableEditorViewModel {
        [Required]
        [StringLength(1024)]
        public string Title { get; set; }
        [StringLength(1024)]
        public string Path { get; set; }
        public int Id { get; set; }
        public string ContentType { get; set; }
        public string BaseUrl { get; set; }
    }
}