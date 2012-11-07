using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace BinaryAnalysis.Navigation {
    public class Routes : IRouteProvider {
        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                                new RouteDescriptor {   Priority = 5,
                                                        Route = new Route(
                                                            "Admin/Navigation",
                                                            new RouteValueDictionary {
                                                                                        {"area", "BinaryAnalysis.Navigation"},
                                                                                        {"controller", "Admin"},
                                                                                        {"action", "Index"}
                                                            },
                                                            new RouteValueDictionary(),
                                                            new RouteValueDictionary {
                                                                                        {"area", "BinaryAnalysis.Navigation"}
                                                            },
                                                            new MvcRouteHandler())
                                }
                            };
        }
    }
}