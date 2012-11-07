using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Orchard.Commands;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Common.Models;
using Orchard.Core.Navigation.Models;
using Orchard.Core.Routable.Models;
using Orchard.Core.Routable.Services;
using Orchard.Security;
using Alois.Timetables.Services;
using Orchard.Core.Navigation.Services;
using Orchard;

namespace Alois.Timetables.Commands {
    public class TimetableCommands : DefaultOrchardCommandHandler {
        private readonly IContentManager _contentManager;
        private readonly IMembershipService _membershipService;
        private readonly ITimetableService _TimetableService;
        private readonly IMenuService _menuService;

        public TimetableCommands(
            IContentManager contentManager,
            IMembershipService membershipService,
            ITimetableService TimetableService,
            IMenuService menuService) {
            _contentManager = contentManager;
            _membershipService = membershipService;
            _TimetableService = TimetableService;
            _menuService = menuService;
        }

        [OrchardSwitch]
        public string FeedUrl { get; set; }

        [OrchardSwitch]
        public string Slug { get; set; }

        [OrchardSwitch]
        public string Owner { get; set; }

        [OrchardSwitch]
        public string Title { get; set; }

        [OrchardSwitch]
        public string MenuText { get; set; }

        [CommandName("timetable create")]
        [CommandHelp("timetable create /Slug:<slug> /Title:<title> /Owner:<username> [/MenuText:<menu text>]\r\n\t" + "Creates a new timetable")]
        [OrchardSwitches("Slug,Title,Owner,MenuText")]
        public string Create() {
            var owner = _membershipService.GetUser(Owner);

            if ( owner == null ) {
                throw new OrchardException(T("Invalid username: {0}", Owner));
            }

            if(!IsSlugValid(Slug)) {
                throw new OrchardException(T("Invalid Slug provided. Timetable creation failed."));
            }

            var Timetable = _contentManager.New("Timetable");
            Timetable.As<ICommonPart>().Owner = owner;
            Timetable.As<RoutePart>().Slug = Slug;
            Timetable.As<RoutePart>().Path = Slug;
            Timetable.As<RoutePart>().Title = Title;
            if ( !String.IsNullOrWhiteSpace(MenuText) ) {
                Timetable.As<MenuPart>().OnMainMenu = true;
                Timetable.As<MenuPart>().MenuPosition = _menuService.Get().Select(menuPart => menuPart.MenuPosition).Max() + 1 + ".0";
                Timetable.As<MenuPart>().MenuText = MenuText;
            }
            _contentManager.Create(Timetable);

            return "Timetable created successfully";
        }

        [CommandName("timetable import")]
        [CommandHelp("timetable import /Slug:<slug> /FeedUrl:<feed url> /Owner:<username>\r\n\t" + "Import all items from <feed url> into the Timetable at the specified <slug>")]
        [OrchardSwitches("FeedUrl,Slug,Owner")]
        public string Import() {
            var owner = _membershipService.GetUser(Owner);

            if(owner == null) {
                throw new OrchardException(T("Invalid username: {0}", Owner));
            }

            XDocument doc;

            try {
                Context.Output.WriteLine("Loading feed...");
                doc = XDocument.Load(FeedUrl);
                Context.Output.WriteLine("Found {0} items", doc.Descendants("item").Count());
            }
            catch ( Exception ex ) {
                throw new OrchardException(T("An error occured while loading the file: {0}", ex.Message));
            }

            var Timetable = _TimetableService.Get(Slug);

            if ( Timetable == null ) {
                throw new OrchardException(T("Timetable not found at specified slug: {0}", Slug));
            }

            foreach ( var item in doc.Descendants("item") ) {
                if (item != null) {
                    var postName = item.Element("title").Value;

                    Context.Output.WriteLine("Adding appointment: {0}...", postName.Substring(0, Math.Min(postName.Length, 40)));
                    var appointment = _contentManager.New("TimetableAppointment");
                    appointment.As<ICommonPart>().Owner = owner;
                    appointment.As<ICommonPart>().Container = Timetable;
                    var slug = Slugify(postName);
                    appointment.As<RoutePart>().Slug = slug;
                    appointment.As<RoutePart>().Path = appointment.As<RoutePart>().GetPathWithSlug(slug);
                    appointment.As<RoutePart>().Title = postName;
                    appointment.As<BodyPart>().Text = item.Element("description").Value;
                    _contentManager.Create(appointment);
                }
            }

            return "Import feed completed.";
        }

        private static string Slugify(string slug) {
            var dissallowed = new Regex(@"[/:?#\[\]@!$&'()*+,;=\s]+");

            slug = dissallowed.Replace(slug, "-");
            slug = slug.Trim('-');

            if ( slug.Length > 1000 )
                slug = slug.Substring(0, 1000);

            return slug.ToLowerInvariant();
        }

        private static bool IsSlugValid(string slug) {
            // see http://tools.ietf.org/html/rfc3987 for prohibited chars
            return slug == null || String.IsNullOrEmpty(slug.Trim()) || Regex.IsMatch(slug, @"^[^/:?#\[\]@!$&'()*+,;=\s]+$");
        }
    }
}