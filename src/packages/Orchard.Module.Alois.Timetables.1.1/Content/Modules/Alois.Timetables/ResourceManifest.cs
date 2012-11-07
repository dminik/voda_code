using Orchard.UI.Resources;

namespace Alois.Timetables {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineStyle("TimetablesAdmin").SetUrl("orchard-timetables-admin.css");
            manifest.DefineStyle("TimetablesArchives").SetUrl("orchard-timetables-archives.css");

            manifest.DefineScript("TimetablesArchives").SetUrl("orchard-timetables-archives.js").SetDependencies("jQuery");
        }
    }
}
