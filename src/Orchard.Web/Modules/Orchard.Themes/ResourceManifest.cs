using Orchard.UI.Resources;

namespace Orchard.Themes {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineStyle("ThemesAdmin").SetUrl("orchard-themes-admin.css");

			manifest.DefineScript("jQuery_metadata").SetUrl("jquery.metadata.js").SetDependencies("jQuery");
			manifest.DefineScript("mbTabset").SetUrl("mbTabset.min.js", "mbTabset.js").SetDependencies("jQuery", "jQueryUI_Core", "jQueryUI_Sortable", "jQuery_metadata");
        }
    }
}
