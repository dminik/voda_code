using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Alois.Timetables.Routing;
using Orchard.Mvc.Routes;

namespace Alois.Timetables {
    public class Routes : IRouteProvider {
        private readonly ITimetableSlugConstraint _TimetableSlugConstraint;

        public Routes(ITimetableSlugConstraint TimetableSlugConstraint)
        {
            _TimetableSlugConstraint = TimetableSlugConstraint;
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "TimetableApi/{TimetableSlug}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableApi"},
                                                                                      {"action", "List"},
                                                                                      {"TimetableSlug", ""}
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"TimetableSlug", _TimetableSlugConstraint}
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "TimetableApi/check",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableApi"},
                                                                                      {"action", "Check"},
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "TimetableApi/{TimetableSlug}/{AppointmentId}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableApi"},
                                                                                      {"action", "Engage"},
                                                                                      {"TimetableSlug", ""}
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"TimetableSlug", _TimetableSlugConstraint}
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Timetables/Create",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableAdmin"},
                                                                                      {"action", "Create"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Timetables/{TimetableId}/Edit",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableAdmin"},
                                                                                      {"action", "Edit"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Timetables/{TimetableId}/Remove",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableAdmin"},
                                                                                      {"action", "Remove"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Timetables/{TimetableId}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableAdmin"},
                                                                                      {"action", "Item"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Timetables/{TimetableId}/Appointments/Create",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableAppointmentAdmin"},
                                                                                      {"action", "Create"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Timetables/{TimetableId}/Appointments/{postId}/Edit",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableAppointmentAdmin"},
                                                                                      {"action", "Edit"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Timetables/{TimetableId}/Appointments/{postId}/Delete",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableAppointmentAdmin"},
                                                                                      {"action", "Delete"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Timetables/{TimetableId}/Appointments/{postId}/Publish",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableAppointmentAdmin"},
                                                                                      {"action", "Publish"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Timetables/{TimetableId}/Appointments/{postId}/Unpublish",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableAppointmentAdmin"},
                                                                                      {"action", "Unpublish"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Timetables",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableAdmin"},
                                                                                      {"action", "List"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Timetables",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "Timetable"},
                                                                                      {"action", "List"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Timetables/Engage/{postId}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableAppointment"},
                                                                                      {"action", "Engage"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "{TimetableSlug}/Archive/{*archiveData}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableAppointment"},
                                                                                      {"action", "ListByArchive"}
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"TimetableSlug", _TimetableSlugConstraint},
                                                                                      {"archiveData", new IsArchiveConstraint()}
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 11,
                                                     Route = new Route(
                                                         "{TimetableSlug}/{postSlug}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "TimetableAppointment"},
                                                                                      {"action", "Item"}
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"TimetableSlug", _TimetableSlugConstraint}
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                    Priority = 11,
                                                    Route = new Route(
                                                         "{TimetableSlug}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"},
                                                                                      {"controller", "Timetable"},
                                                                                      {"action", "Item"},
                                                                                      {"TimetableSlug", ""}
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"TimetableSlug", _TimetableSlugConstraint}
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"area", "Alois.Timetables"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }
    }
}