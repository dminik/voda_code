using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.UI.Resources;

namespace Mod.Colorbox
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            var mainScript = "colorbox";

            manifest.DefineScript(mainScript)
                .SetUrl("jquery.colorbox.js")
                .SetDependencies("jQuery");

            manifest.DefineStyle("colorbox.default")
                .SetUrl("colorbox.css");
        }
    }
}
