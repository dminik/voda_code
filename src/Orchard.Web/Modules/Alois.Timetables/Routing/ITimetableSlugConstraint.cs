using System.Collections.Generic;
using System.Web.Routing;
using Orchard;

namespace Alois.Timetables.Routing {
    public interface ITimetableSlugConstraint : IRouteConstraint, ISingletonDependency {
        void SetSlugs(IEnumerable<string> slugs);
        string FindSlug(string slug);
        void AddSlug(string slug);
        void RemoveSlug(string slug);
    }
}