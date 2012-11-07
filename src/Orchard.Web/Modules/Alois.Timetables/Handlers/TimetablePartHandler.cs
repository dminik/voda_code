using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using JetBrains.Annotations;
using Alois.Timetables.Models;
using Alois.Timetables.Routing;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Routable.Models;
using Orchard.Core.Routable.Services;
using Orchard.Data;
using Orchard.Services;
using Orchard;

namespace Alois.Timetables.Handlers {
    [UsedImplicitly]
    public class TimetablePartHandler : ContentHandler {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly ITimetableSlugConstraint _TimetableSlugConstraint;
        private readonly IHomePageProvider _routableHomePageProvider;

        public TimetablePartHandler(IRepository<TimetablePartRecord> repository, IWorkContextAccessor workContextAccessor, IEnumerable<IHomePageProvider> homePageProviders, ITimetableSlugConstraint TimetableSlugConstraint) {
            _workContextAccessor = workContextAccessor;
            _TimetableSlugConstraint = TimetableSlugConstraint;
            _routableHomePageProvider = homePageProviders.SingleOrDefault(p => p.GetProviderName() == RoutableHomePageProvider.Name);
            Filters.Add(StorageFilter.For(repository));

            Action<PublishContentContext, RoutePart> publishedHandler = (context, route) => {
                if (route.Is<TimetablePart>()) {
                    if (route.ContentItem.Id != 0 && route.PromoteToHomePage)
                        _TimetableSlugConstraint.AddSlug("");
                }
                else if (route.ContentItem.Id != 0 && route.PromoteToHomePage) {
                    _TimetableSlugConstraint.RemoveSlug("");
                }
            };

            OnPublished<RoutePart>(publishedHandler);
            OnUnpublished<RoutePart>(publishedHandler);

            OnGetDisplayShape<TimetablePart>((context, Timetable) => {
                context.Shape.Description = Timetable.Description;
                context.Shape.AppointmentCount = Timetable.AppointmentCount;
            });
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            var Timetable = context.ContentItem.As<TimetablePart>();

            if (Timetable == null)
                return;

            var TimetableSlug = Timetable.Id == _routableHomePageProvider.GetHomePageId(_workContextAccessor.GetContext().CurrentSite.HomePage)
                ? ""
                : Timetable.As<RoutePart>().Slug;

            context.Metadata.DisplayRouteValues = new RouteValueDictionary {
                {"Area", "Alois.Timetables"},
                {"Controller", "Timetable"},
                {"Action", "Item"},
                {"TimetableSlug", TimetableSlug}
            };
            context.Metadata.CreateRouteValues = new RouteValueDictionary {
                {"Area", "Alois.Timetables"},
                {"Controller", "TimetableAdmin"},
                {"Action", "Create"}
            };
            context.Metadata.EditorRouteValues = new RouteValueDictionary {
                {"Area", "Alois.Timetables"},
                {"Controller", "TimetableAdmin"},
                {"Action", "Edit"},
                {"TimetableId", context.ContentItem.Id}
            };
            context.Metadata.RemoveRouteValues = new RouteValueDictionary {
                {"Area", "Alois.Timetables"},
                {"Controller", "TimetableAdmin"},
                {"Action", "Remove"},
                {"TimetableId", context.ContentItem.Id}
            };
        }
    }
}