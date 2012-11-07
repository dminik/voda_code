using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Records;


namespace Orchard.SEO.Models
{
    public class MetaRecord: ContentPartRecord {
        public virtual string Keywords { get; set; }
        public virtual string Description { get; set; }
    }
}
