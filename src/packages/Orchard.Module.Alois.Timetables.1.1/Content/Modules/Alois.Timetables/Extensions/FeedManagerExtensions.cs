using System.Web.Routing;
using Alois.Timetables.Models;
using Orchard.Core.Feeds;

namespace Alois.Timetables.Extensions {
    public static class FeedManagerExtensions {
        public static void Register(this IFeedManager feedManager, TimetablePart TimetablePart) {
            feedManager.Register(TimetablePart.Name, "rss", new RouteValueDictionary { { "containerid", TimetablePart.Id } });
            feedManager.Register(TimetablePart.Name + " - Comments", "rss", new RouteValueDictionary { { "commentedoncontainer", TimetablePart.Id } });
        }
    }
}
