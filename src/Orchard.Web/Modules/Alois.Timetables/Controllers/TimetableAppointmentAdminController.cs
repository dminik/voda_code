using System;
using System.Reflection;
using System.Web.Mvc;
using Alois.Timetables.Extensions;
using Alois.Timetables.Models;
using Alois.Timetables.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Contents.Settings;
using Orchard.Localization;
using Orchard.Mvc.AntiForgery;
using Orchard.Mvc.Extensions;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using Orchard;

namespace Alois.Timetables.Controllers {
    [ValidateInput(false), Admin]
    public class TimetableAppointmentAdminController : Controller, IUpdateModel {
        private readonly ITimetableService _TimetableService;
        private readonly ITimetableAppointmentService _TimetableAppointmentService;

        public TimetableAppointmentAdminController(IOrchardServices services, ITimetableService TimetableService, ITimetableAppointmentService TimetableAppointmentService) {
            Services = services;
            _TimetableService = TimetableService;
            _TimetableAppointmentService = TimetableAppointmentService;
            T = NullLocalizer.Instance;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult Create(int TimetableId) {
            if (!Services.Authorizer.Authorize(Permissions.EditTimetableAppointment, T("Not allowed to create timetable appointment")))
                return new HttpUnauthorizedResult();

            var Timetable = _TimetableService.Get(TimetableId, VersionOptions.Latest).As<TimetablePart>();
            if (Timetable == null)
                return HttpNotFound();

            var TimetableAppointment = Services.ContentManager.New<TimetableAppointmentPart>("TimetableAppointment");
            TimetableAppointment.TimetablePart = Timetable;

            dynamic model = Services.ContentManager.BuildEditor(TimetableAppointment);
            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }

        [HttpPost, ActionName("Create")]
        [FormValueRequired("submit.Save")]
        public ActionResult CreatePOST(int TimetableId) {
            return CreatePOST(TimetableId, contentItem => {
                if (!contentItem.Has<IPublishingControlAspect>() && !contentItem.TypeDefinition.Settings.GetModel<ContentTypeSettings>().Draftable)
                    Services.ContentManager.Publish(contentItem);
            });
        }

        [HttpPost, ActionName("Create")]
        [FormValueRequired("submit.Publish")]
        public ActionResult CreateAndPublishPOST(int TimetableId) {
            if (!Services.Authorizer.Authorize(Permissions.PublishTimetableAppointment, T("Couldn't create Timetable appointment")))
                return new HttpUnauthorizedResult();

            return CreatePOST(TimetableId, contentItem => Services.ContentManager.Publish(contentItem));
        }

        private ActionResult CreatePOST(int TimetableId, Action<ContentItem> conditionallyPublish) {
            if (!Services.Authorizer.Authorize(Permissions.EditTimetableAppointment, T("Couldn't create Timetable appointment")))
                return new HttpUnauthorizedResult();

            var Timetable = _TimetableService.Get(TimetableId, VersionOptions.Latest).As<TimetablePart>();
            if (Timetable == null)
                return HttpNotFound();

            var TimetableAppointment = Services.ContentManager.New<TimetableAppointmentPart>("TimetableAppointment");
            TimetableAppointment.TimetablePart = Timetable;

            Services.ContentManager.Create(TimetableAppointment, VersionOptions.Draft);
            var model = Services.ContentManager.UpdateEditor(TimetableAppointment, this);

            if (!ModelState.IsValid) {
                Services.TransactionManager.Cancel();
                // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
                return View((object)model);
            }

            conditionallyPublish(TimetableAppointment.ContentItem);

            Services.Notifier.Information(T("Your {0} has been created.", TimetableAppointment.TypeDefinition.DisplayName));
            return Redirect(Url.TimetableAppointmentEdit(TimetableAppointment));
        }

        //todo: the content shape template has extra bits that the core contents module does not (remove draft functionality)
        //todo: - move this extra functionality there or somewhere else that's appropriate?
        public ActionResult Edit(int TimetableId, int postId) {
            var Timetable = _TimetableService.Get(TimetableId, VersionOptions.Latest);
            if (Timetable == null)
                return HttpNotFound();

            var appointment = _TimetableAppointmentService.Get(postId, VersionOptions.Latest);
            if (appointment == null)
                return HttpNotFound();

            if (!Services.Authorizer.Authorize(Permissions.EditTimetableAppointment, appointment, T("Couldn't edit timetable appointment")))
                return new HttpUnauthorizedResult();

            dynamic model = Services.ContentManager.BuildEditor(appointment);
            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("submit.Save")]
        public ActionResult EditPOST(int TimetableId, int postId, string returnUrl) {
            return EditPOST(TimetableId, postId, returnUrl, contentItem => {
                if (!contentItem.Has<IPublishingControlAspect>() && !contentItem.TypeDefinition.Settings.GetModel<ContentTypeSettings>().Draftable)
                    Services.ContentManager.Publish(contentItem);
            });
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("submit.Publish")]
        public ActionResult EditAndPublishPOST(int TimetableId, int postId, string returnUrl) {
            var Timetable = _TimetableService.Get(TimetableId, VersionOptions.Latest);
            if (Timetable == null)
                return HttpNotFound();

            // Get draft (create a new version if needed)
            var TimetableAppointment = _TimetableAppointmentService.Get(postId, VersionOptions.DraftRequired);
            if (TimetableAppointment == null)
                return HttpNotFound();

            if (!Services.Authorizer.Authorize(Permissions.PublishTimetableAppointment, TimetableAppointment, T("Couldn't publish Timetable appointment")))
                return new HttpUnauthorizedResult();

            return EditPOST(TimetableId, postId, returnUrl, contentItem => Services.ContentManager.Publish(contentItem));
        }

        public ActionResult EditPOST(int TimetableId, int postId, string returnUrl, Action<ContentItem> conditionallyPublish) {
            var Timetable = _TimetableService.Get(TimetableId, VersionOptions.Latest);
            if (Timetable == null)
                return HttpNotFound();

            // Get draft (create a new version if needed)
            var TimetableAppointment = _TimetableAppointmentService.Get(postId, VersionOptions.DraftRequired);
            if (TimetableAppointment == null)
                return HttpNotFound();

            if (!Services.Authorizer.Authorize(Permissions.EditTimetableAppointment, TimetableAppointment, T("Couldn't edit Timetable appointment")))
                return new HttpUnauthorizedResult();

            // Validate form input
            var model = Services.ContentManager.UpdateEditor(TimetableAppointment, this);
            if (!ModelState.IsValid) {
                Services.TransactionManager.Cancel();
                // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
                return View((object)model);
            }

            conditionallyPublish(TimetableAppointment.ContentItem);

            Services.Notifier.Information(T("Your {0} has been saved.", TimetableAppointment.TypeDefinition.DisplayName));

            return this.RedirectLocal(returnUrl, Url.TimetableAppointmentEdit(TimetableAppointment));
        }

        [ValidateAntiForgeryTokenOrchard]
        public ActionResult DiscardDraft(int id) {
            // get the current draft version
            var draft = Services.ContentManager.Get(id, VersionOptions.Draft);
            if (draft == null) {
                Services.Notifier.Information(T("There is no draft to discard."));
                return RedirectToEdit(id);
            }

            // check edit permission
            if (!Services.Authorizer.Authorize(Permissions.EditTimetableAppointment, draft, T("Couldn't discard timetable appointment draft")))
                return new HttpUnauthorizedResult();

            // locate the published revision to revert onto
            var published = Services.ContentManager.Get(id, VersionOptions.Published);
            if (published == null) {
                Services.Notifier.Information(T("Can not discard draft on unpublished timetable appointment."));
                return RedirectToEdit(draft);
            }

            // marking the previously published version as the latest
            // has the effect of discarding the draft but keeping the history
            draft.VersionRecord.Latest = false;
            published.VersionRecord.Latest = true;

            Services.Notifier.Information(T("Timetable appointment draft version discarded"));
            return RedirectToEdit(published);
        }

        ActionResult RedirectToEdit(int id) {
            return RedirectToEdit(Services.ContentManager.GetLatest<TimetableAppointmentPart>(id));
        }

        ActionResult RedirectToEdit(IContent item) {
            if (item == null || item.As<TimetableAppointmentPart>() == null)
                return HttpNotFound();
            return RedirectToAction("Edit", new { TimetableId = item.As<TimetableAppointmentPart>().TimetablePart.Id, AppointmentId = item.ContentItem.Id });
        }

        [ValidateAntiForgeryTokenOrchard]
        public ActionResult Delete(int TimetableId, int postId) {
            //refactoring: test PublishTimetableAppointment/PublishTimetableAppointment in addition if published

            var Timetable = _TimetableService.Get(TimetableId, VersionOptions.Latest);
            if (Timetable == null)
                return HttpNotFound();

            var appointment = _TimetableAppointmentService.Get(postId, VersionOptions.Latest);
            if (appointment == null)
                return HttpNotFound();

            if (!Services.Authorizer.Authorize(Permissions.DeleteTimetableAppointment, appointment, T("Couldn't delete Timetable appointment")))
                return new HttpUnauthorizedResult();

            _TimetableAppointmentService.Delete(appointment);
            Services.Notifier.Information(T("Timetable appointment was successfully deleted"));

            return Redirect(Url.TimetableForAdmin(Timetable.As<TimetablePart>()));
        }

        [ValidateAntiForgeryTokenOrchard]
        public ActionResult Publish(int TimetableId, int postId) {
            var Timetable = _TimetableService.Get(TimetableId, VersionOptions.Latest);
            if (Timetable == null)
                return HttpNotFound();

            var appointment = _TimetableAppointmentService.Get(postId, VersionOptions.Latest);
            if (appointment == null)
                return HttpNotFound();

            if (!Services.Authorizer.Authorize(Permissions.PublishTimetableAppointment, appointment, T("Couldn't publish timetable appointment")))
                return new HttpUnauthorizedResult();

            _TimetableAppointmentService.Publish(appointment);
            Services.Notifier.Information(T("Timetable appointment successfully published."));

            return Redirect(Url.TimetableForAdmin(Timetable.As<TimetablePart>()));
        }

        [ValidateAntiForgeryTokenOrchard]
        public ActionResult Unpublish(int TimetableId, int postId) {
            var Timetable = _TimetableService.Get(TimetableId, VersionOptions.Latest);
            if (Timetable == null)
                return HttpNotFound();

            var appointment = _TimetableAppointmentService.Get(postId, VersionOptions.Latest);
            if (appointment == null)
                return HttpNotFound();

            if (!Services.Authorizer.Authorize(Permissions.PublishTimetableAppointment, appointment, T("Couldn't unpublish Timetable appointment")))
                return new HttpUnauthorizedResult();

            _TimetableAppointmentService.Unpublish(appointment);
            Services.Notifier.Information(T("Timetable appointment successfully unpublished."));

            return Redirect(Url.TimetableForAdmin(Timetable.As<TimetablePart>()));
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }

    public class FormValueRequiredAttribute : ActionMethodSelectorAttribute {
        private readonly string _submitButtonName;

        public FormValueRequiredAttribute(string submitButtonName) {
            _submitButtonName = submitButtonName;
        }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo) {
            var value = controllerContext.HttpContext.Request.Form[_submitButtonName];
            return !string.IsNullOrEmpty(value);
        }
    }
}