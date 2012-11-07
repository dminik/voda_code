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
using Orchard;
using Alois.EventDate.Models;
using Alois.Timetables.Models;

namespace Alois.Timetables.Controllers {

    [Themed]
    public class TimetableController : Controller {
        private readonly IOrchardServices _services;
        private readonly ITimetableService _TimetableService;
        private readonly ITimetableAppointmentService _TimetableAppointmentService;
        private readonly ITimetableSlugConstraint _TimetableSlugConstraint;
        private readonly IFeedManager _feedManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IHomePageProvider _routableHomePageProvider;
        private readonly ISiteService _siteService;

        public TimetableController(
            IOrchardServices services, 
            ITimetableService TimetableService,
            ITimetableAppointmentService TimetableAppointmentService,
            ITimetableSlugConstraint TimetableSlugConstraint,
            IFeedManager feedManager, 
            IShapeFactory shapeFactory,
            IWorkContextAccessor workContextAccessor,
            IEnumerable<IHomePageProvider> homePageProviders,
            ISiteService siteService) {
            _services = services;
            _TimetableService = TimetableService;
            _TimetableAppointmentService = TimetableAppointmentService;
            _TimetableSlugConstraint = TimetableSlugConstraint;
            _feedManager = feedManager;
            _workContextAccessor = workContextAccessor;
            _siteService = siteService;
            _routableHomePageProvider = homePageProviders.SingleOrDefault(p => p.GetProviderName() == RoutableHomePageProvider.Name);
            Logger = NullLogger.Instance;
            Shape = shapeFactory;
        }

        dynamic Shape { get; set; }
        protected ILogger Logger { get; set; }

        public ActionResult List() {
            var Timetables = _TimetableService.Get().Select(b => _services.ContentManager.BuildDisplay(b, "Summary"));

            var list = Shape.List();
            list.AddRange(Timetables);

            dynamic viewModel = Shape.ViewModel()
                .ContentItems(list);

            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)viewModel);
        }

        public ActionResult Item(string TimetableSlug, PagerParameters pagerParameters) {
            Pager pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

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
            var TimetableAppointments = _TimetableAppointmentService.Get(TimetablePart, pager.GetStartIndex(), pager.PageSize)
                //.Select(b => _services.ContentManager.BuildDisplay(b, "Summary"));
                .Select(b => b);

            dynamic Timetable = _services.ContentManager.BuildDisplay(TimetablePart);

            var list = Shape.List();
            list.AddRange(TimetableAppointments);
            Timetable.Content.Add(Shape.Parts_Timetables_TimetableAppointment_List(ContentItems: list), "5");

            var totalItemCount = _TimetableAppointmentService.AppointmentCount(TimetablePart);
            Timetable.Content.Add(Shape.Pager(pager).TotalItemCount(totalItemCount), "Content:after");

            return View((object)Timetable);
        }
    }
}