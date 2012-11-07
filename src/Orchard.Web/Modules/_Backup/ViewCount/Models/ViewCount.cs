using System;
using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace ViewCount.Models
{
    public class ViewCountRecord : ContentPartRecord
    {
        public virtual int ViewCount { get; set; }
    }

    public class ViewCountPart : ContentPart<ViewCountRecord>
    {
        [Required]
        public int ViewCount
        {
            get { return Record.ViewCount; }
            set { Record.ViewCount = value; }
        }
    }
}