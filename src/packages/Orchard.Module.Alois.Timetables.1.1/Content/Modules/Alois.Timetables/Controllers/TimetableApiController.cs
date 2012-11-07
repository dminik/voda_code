using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Alois.Timetables.Extensions;
using Alois.Timetables.Routing;
using Alois.Timetables.Services;
using Orchard.Core.Feeds;
using Orchard.Core.Routable.Services;
using Orchard.DisplayManagement;
using Orchard.Logging;
using Orchard.Services;
using Orchard.Themes;
using Orchard.UI.Navigation;
using Orchard.Settings;
using Orchard.Security;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Data;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using Alois.EventDate.Models;
using Alois.Timetables.Models;

namespace Alois.Timetables.Controllers {

    [Themed]
    public class TimetableApiController : Controller {
        private readonly IOrchardServices _services;
        private readonly ITimetableService _TimetableService;
        private readonly ITimetableAppointmentService _TimetableAppointmentService;
        private readonly ITimetableSlugConstraint _TimetableSlugConstraint;
        private readonly IFeedManager _feedManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IHomePageProvider _routableHomePageProvider;
        private readonly ISiteService _siteService;
        private readonly IAuthenticationService _authService;

        public TimetableApiController(
            IOrchardServices services, 
            ITimetableService TimetableService,
            ITimetableAppointmentService TimetableAppointmentService,
            ITimetableSlugConstraint TimetableSlugConstraint,
            IFeedManager feedManager, 
            IShapeFactory shapeFactory,
            IWorkContextAccessor workContextAccessor,
            IEnumerable<IHomePageProvider> homePageProviders,
            ISiteService siteService,
            IAuthenticationService authService) {
            _services = services;
            _TimetableService = TimetableService;
            _TimetableAppointmentService = TimetableAppointmentService;
            _TimetableSlugConstraint = TimetableSlugConstraint;
            _feedManager = feedManager;
            _workContextAccessor = workContextAccessor;
            _siteService = siteService;
            _routableHomePageProvider = homePageProviders.SingleOrDefault(p => p.GetProviderName() == RoutableHomePageProvider.Name );
            Logger = NullLogger.Instance;
            Shape = shapeFactory;
            _authService = authService;
        }

        dynamic Shape { get; set; }
        protected ILogger Logger { get; set; }
        
        
        
        private TimetableAppointmentPart CheckInternal() {
            var me = _authService.GetAuthenticatedUser();
            var Timetables = _TimetableService.Get();
            foreach( var timetable in Timetables ) {
                var TimetableAppointments = _TimetableAppointmentService.Get(timetable, VersionOptions.Latest).Where(x => x.Creator==me  && x.Date.ScheduledPublishUtc.Value >= DateTime.Now );
            
            if( TimetableAppointments.Count() > 0 )
                return TimetableAppointments.First();
            }
            return null;
        }

        [Authorize]
        public ActionResult Check() {
            var ap = CheckInternal();
                
            if( ap == null )
                return HttpNotFound();
            
            string all = '{' + string.Format(" \"id\": {0}, \"Date\": \"{1}\", \"time\": \"{2}\"", 
                ap.Id, 
                ap.Date.ScheduledPublishUtc.Value.ToString("MM:dd:yyyy"), 
                ap.Date.ScheduledPublishUtc.Value.ToString("HH:mm")
            ) + '}';
            
            return Content(all,@"text/html");
        }

        public ActionResult List(string TimetableSlug) {

            var correctedSlug = _TimetableSlugConstraint.FindSlug(TimetableSlug);
            if (correctedSlug == null)
                return HttpNotFound();

            var TimetablePart = _TimetableService.Get(correctedSlug);
            if (TimetablePart == null)
                return HttpNotFound();

            if (!RouteData.DataTokens.ContainsKey("ParentActionViewContext")
                && TimetablePart.Id == _routableHomePageProvider.GetHomePageId(_workContextAccessor.GetContext().CurrentSite.HomePage)) {
                return HttpNotFound();
            }
            _feedManager.Register(TimetablePart);
            var TimetableAppointments = _TimetableAppointmentService.Get(TimetablePart)
                .Select(b => new {
                        Id = b.Id,
                        Date = b.Date.ScheduledPublishUtc.Value.Date,
                        Time = b.Date.ScheduledPublishUtc.Value
                }).GroupBy( x => x.Date );
            
                
            string all = "";
            foreach( var day in TimetableAppointments ) {
                string aps = "";
                foreach( var ap in day ) {
                    aps += string.Format("<li class=\"appointment-{0} appointment\">" +
                        "<div class=\"start\">{1}</div>" +
                        "<div class=\"duration\">30 minutes</div>" +
                        "</li>", ap.Id, ap.Time.ToString("HH:mm") );
                }
                all += string.Format("<div class=\"day\">" +
                    "<h3>{0}.</h3>" + "<ul>{1}</ul><div>", day.First().Date.ToString("MM/dd/yyyy"), aps );
            }
            
            return Content(all, @"text/html");
        }
        
        [Authorize]
        public ActionResult Engage(string TimeTableSlug, int AppointmentId) {
            TimetableAppointmentPart ap = _TimetableAppointmentService.Get(AppointmentId);
            if (ap == null)
                return HttpNotFound();
            
            var me = _authService.GetAuthenticatedUser();
            if( ap.Creator == me ) {
                ap.ContentItem.ContentManager.Publish(ap.ContentItem);
                ap.Creator = ap.TimetablePart.Creator;
            }else{
                ap.ContentItem.ContentManager.Unpublish(ap.ContentItem);
                ap.Creator = _authService.GetAuthenticatedUser();
            }
            
            return Content("","");
        }
    }
}