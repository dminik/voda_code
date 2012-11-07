using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Alois.Timetables.Extensions;
using Alois.Timetables.Models;
using Alois.Timetables.Routing;
using Alois.Timetables.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Routable.Services;
using Orchard.Data;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using Orchard.Settings;
using Orchard;

namespace Alois.Timetables.Controllers {

    [ValidateInput(false), Admin]
    public class TimetableAdminController : Controller, IUpdateModel {
        private readonly ITimetableService _TimetableService;
        private readonly ITimetableAppointmentService _TimetableAppointmentService;
        private readonly IContentManager _contentManager;
        private readonly ITransactionManager _transactionManager;
        private readonly ITimetableSlugConstraint _TimetableSlugConstraint;
        private readonly ISiteService _siteService;

        public TimetableAdminController(
            IOrchardServices services,
            ITimetableService TimetableService,
            ITimetableAppointmentService TimetableAppointmentService,
            IContentManager contentManager,
            ITransactionManager transactionManager,
            ITimetableSlugConstraint TimetableSlugConstraint,
            ISiteService siteService,
            IShapeFactory shapeFactory) {
            Services = services;
            _TimetableService = TimetableService;
            _TimetableAppointmentService = TimetableAppointmentService;
            _contentManager = contentManager;
            _transactionManager = transactionManager;
            _TimetableSlugConstraint = TimetableSlugConstraint;
            _siteService = siteService;
            T = NullLocalizer.Instance;
            Shape = shapeFactory;
        }

        dynamic Shape { get; set; }
        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }

        public ActionResult Create() {
            if (!Services.Authorizer.Authorize(Permissions.ManageTimetables, T("Not allowed to create timetables")))
                return new HttpUnauthorizedResult();

            TimetablePart Timetable = Services.ContentManager.New<TimetablePart>("Timetable");
            if (Timetable == null)
                return HttpNotFound();

            dynamic model = Services.ContentManager.BuildEditor(Timetable);
            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePOST() {
            if (!Services.Authorizer.Authorize(Permissions.ManageTimetables, T("Couldn't create timetable")))
                return new HttpUnauthorizedResult();

            var Timetable = Services.ContentManager.New<TimetablePart>("Timetable");

            _contentManager.Create(Timetable, VersionOptions.Draft);
            dynamic model = _contentManager.UpdateEditor(Timetable, this);

            if (!ModelState.IsValid) {
                _transactionManager.Cancel();
                // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
                return View((object)model);
            }

            _contentManager.Publish(Timetable.ContentItem);
            _TimetableSlugConstraint.AddSlug(Timetable.As<IRoutableAspect>().GetEffectiveSlug());

            return Redirect(Url.TimetableForAdmin(Timetable));
        }

        public ActionResult Edit(int TimetableId) {
            if (!Services.Authorizer.Authorize(Permissions.ManageTimetables, T("Not allowed to edit timetable")))
                return new HttpUnauthorizedResult();

            var Timetable = _TimetableService.Get(TimetableId, VersionOptions.Latest);
            if (Timetable == null)
                return HttpNotFound();

            dynamic model = Services.ContentManager.BuildEditor(Timetable);
            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPOST(int TimetableId) {
            if (!Services.Authorizer.Authorize(Permissions.ManageTimetables, T("Couldn't edit timetable")))
                return new HttpUnauthorizedResult();

            var Timetable = _TimetableService.Get(TimetableId, VersionOptions.DraftRequired);
            if (Timetable == null)
                return HttpNotFound();

            dynamic model = Services.ContentManager.UpdateEditor(Timetable, this);
            if (!ModelState.IsValid) {
                Services.TransactionManager.Cancel();
                // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
                return View((object)model);
            }

            _contentManager.Publish(Timetable);
            _TimetableSlugConstraint.AddSlug(Timetable.As<IRoutableAspect>().GetEffectiveSlug());
            Services.Notifier.Information(T("Timetable information updated"));

            return Redirect(Url.TimetablesForAdmin());
        }

        [HttpPost]
        public ActionResult Remove(int TimetableId) {
            if (!Services.Authorizer.Authorize(Permissions.ManageTimetables, T("Couldn't delete timetable")))
                return new HttpUnauthorizedResult();

            var Timetable = _TimetableService.Get(TimetableId, VersionOptions.Latest);

            if (Timetable == null)
                return HttpNotFound();

            _TimetableService.Delete(Timetable);

            Services.Notifier.Information(T("Timetable was successfully deleted"));
            return Redirect(Url.TimetablesForAdmin());
        }

        public ActionResult List() {
            var list = Services.New.List();
            list.AddRange(_TimetableService.Get(VersionOptions.Latest)
                              .Select(b => {
                                          var Timetable = Services.ContentManager.BuildDisplay(b, "SummaryAdmin");
                                          Timetable.TotalAppointmentCount = _TimetableAppointmentService.Get(b, VersionOptions.Latest).Count();
                                          return Timetable;
                                      }));

            dynamic viewModel = Services.New.ViewModel()
                .ContentItems(list);
            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)viewModel);
        }

        public ActionResult Item(int TimetableId, PagerParameters pagerParameters) {
            Pager pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);
            TimetablePart TimetablePart = _TimetableService.Get(TimetableId, VersionOptions.Latest).As<TimetablePart>();

            if (TimetablePart == null)
                return HttpNotFound();

            var TimetableAppointments = _TimetableAppointmentService.Get(TimetablePart, pager.GetStartIndex(), pager.PageSize, VersionOptions.Latest)
                .Select(bp => _contentManager.BuildDisplay(bp, "SummaryAdmin"));

            dynamic Timetable = Services.ContentManager.BuildDisplay(TimetablePart, "DetailAdmin");

            var list = Shape.List();
            list.AddRange(TimetableAppointments);
            Timetable.Content.Add(Shape.Parts_Timetables_TimetableAppointment_ListAdmin(ContentItems: list), "5");

            var totalItemCount = _TimetableAppointmentService.AppointmentCount(TimetablePart, VersionOptions.Latest);
            Timetable.Content.Add(Shape.Pager(pager).TotalItemCount(totalItemCount), "Content:after");

            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)Timetable);
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}