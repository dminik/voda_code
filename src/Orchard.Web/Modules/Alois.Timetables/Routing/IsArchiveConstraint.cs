using System.Web;
using System.Web.Routing;
using Alois.Timetables.Models;

namespace Alois.Timetables.Routing {
    public class IsArchiveConstraint : IRouteConstraint {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
                          RouteDirection routeDirection) {
            if(values[parameterName] == null) {
                return false;
            }

            try {
                var archiveData = new ArchiveData(values[parameterName].ToString());
                archiveData.ToDateTime();
                return true;
            }
            catch {
                return false;
            }
        }
    }
}