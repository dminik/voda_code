using System.Linq;
using System.Web.Mvc;
using Alois.Timetables.Extensions;
using Alois.Timetables.Models;
using Alois.Timetables.Services;
using Orchard.ContentManagement;
using Orchard.Core.Feeds;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Themes;
using Orchard;

namespace Alois.Timetables.Controllers {
    [Themed]
    public class TimetableAppointmentController : Controller {
        private readonly IOrchardServices _services;
        private readonly ITimetableService _TimetableService;
        private readonly ITimetableAppointmentService _TimetableAppointmentService;
        private readonly IFeedManager _feedManager;
        private readonly IAuthenticationService _authService;

        public TimetableAppointmentController(
            IOrchardServices services, 
            ITimetableService TimetableService, 
            ITimetableAppointmentService TimetableAppointmentService,
            IFeedManager feedManager,
            IShapeFactory shapeFactory,
            IAuthenticationService authService) {
            _services = services;
            _TimetableService = TimetableService;
            _TimetableAppointmentService = TimetableAppointmentService;
            _feedManager = feedManager;
            _authService = authService;
            T = NullLocalizer.Instance;
            Shape = shapeFactory;
        }

        dynamic Shape { get; set; }
        public Localizer T { get; set; }

        public ActionResult Item(string TimetableSlug, string postSlug) {
            if (!_services.Authorizer.Authorize(StandardPermissions.AccessFrontEnd, T("Couldn't view Timetable appointment")))
                return new HttpUnauthorizedResult();

            var TimetablePart = _TimetableService.Get(TimetableSlug);
            if (TimetablePart == null)
                return HttpNotFound();

            var postPart = _TimetableAppointmentService.Get(TimetablePart, postSlug, VersionOptions.Published);
            if (postPart == null)
                return HttpNotFound();

            dynamic model = _services.ContentManager.BuildDisplay(postPart);
            return View((object)model);
        }

        [Authorize]
        public ActionResult Engage(string postId)
        {
            TimetableAppointmentPart app = _TimetableAppointmentService.Get(int.Parse(postId));
            if (app == null)
                return HttpNotFound();
            app.ContentItem.ContentManager.Unpublish(app.ContentItem);
            app.Creator = _authService.GetAuthenticatedUser();
            dynamic model = app;
            return View((object)model);
        }

        public ActionResult ListByArchive(string TimetableSlug, string archiveData) {
            TimetablePart TimetablePart = _TimetableService.Get(TimetableSlug);

            if (TimetablePart == null)
                return HttpNotFound();

            var archive = new ArchiveData(archiveData);

            var list = Shape.List();
            list.AddRange(_TimetableAppointmentService.Get(TimetablePart, archive).Select(b => _services.ContentManager.BuildDisplay(b, "Summary")));

            _feedManager.Register(TimetablePart);

            dynamic viewModel = Shape.ViewModel()
                .ContentItems(list)
                .Timetable(TimetablePart)
                .ArchiveData(archive);

            return View((object)viewModel);
        }
    }
}