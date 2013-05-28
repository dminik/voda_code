using System;
using System.Linq;
using Orchard.Alias;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;
using Orchard.Routable.Models;
using Orchard.Routable.Services;

namespace Orchard.Core.Routable.Handlers {
    public class RoutePartHandler : ContentHandler {
        private readonly IRoutableService _routableService;

        public RoutePartHandler(
            IRoutableService routableService,
            IRepository<RoutePart2Record> repository) {
            Filters.Add(StorageFilter.For(repository));

            _routableService = routableService;
            T = NullLocalizer.Instance;

            OnGetDisplayShape<RoutePart2>(SetModelProperties);
            OnGetEditorShape<RoutePart2>(SetModelProperties);
            OnUpdateEditorShape<RoutePart2>(SetModelProperties);

            OnRemoved<RoutePart2>((context, route) => _routableService.RemoveAliases(route.ContentItem));
            OnIndexing<RoutePart2>((context, part) => context.DocumentIndex.Add("title", part.Record.Title).RemoveTags().Analyze());
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            var part = context.ContentItem.As<RoutePart2>();

            if (part != null) {
                // todo: what is this for?
                //context.Metadata.Identity.Add("Route.Path", part.Slug);
            }
        }

        private static void SetModelProperties(BuildShapeContext context, RoutePart2 route) {
            context.Shape.Title = route.Record.Title;
            // todo: .Path used to be here, what was this for?
        }
    }
}
