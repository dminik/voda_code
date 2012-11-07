using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;


namespace Orchard.SEO.Models
{
    public class MetaPart : ContentPart<MetaRecord>
    {
        public string Keywords
        {
            get { return Record.Keywords; }
            set { Record.Keywords = value; }
        }

        public string Description
        {
            get { return Record.Description; }
            set { Record.Description = value; }
        }
    
    }
}