using System.Collections.Generic;
using Orchard.ContentManagement;

namespace Orchard.Routable.Services {
    public interface IRoutableService : IDependency {
        bool IsPathValid(string path);
        void RemoveAliases(ContentItem content);
        void SetAlias(ContentItem content, string path);
        string GetAlias(ContentItem content);
        IEnumerable<string> GetAliases(ContentItem content);
    }
}