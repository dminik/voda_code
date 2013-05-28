using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Orchard.Alias;
using Orchard.ContentManagement;

namespace Orchard.Routable.Services {
    public class RoutableService : IRoutableService
    {
        private readonly IAliasService _aliasService;

        public RoutableService(IAliasService aliasService)
        {
            _aliasService = aliasService;
        }

        public bool IsPathValid(string path)
        {
            return String.IsNullOrWhiteSpace(path) || Regex.IsMatch(path, @"^[^:?#\[\]@!$&'()*+,;=\s\""\<\>\\]+$") && !(path.StartsWith(".") || path.EndsWith("."));
        }


        public void RemoveAliases(ContentItem content)
        {
            var rvd = content.ContentManager.GetItemMetadata(content).DisplayRouteValues;
            foreach (var alias in _aliasService.Lookup(rvd))
            {
                _aliasService.Delete(alias);
            }
        }

        public void SetAlias(ContentItem content, string path)
        {
            var rvd = content.ContentManager.GetItemMetadata(content).DisplayRouteValues;
            // remove old aliases that are different than the new one
            var oldAliases = _aliasService.Lookup(rvd).Where(a => !a.Equals(path));
            foreach (var alias in oldAliases)
            {
                _aliasService.Delete(alias);
            }
            // create the new alias
            _aliasService.Set(path, rvd);
        }

        public string GetAlias(ContentItem content)
        {
            var rvd = content.ContentManager.GetItemMetadata(content).DisplayRouteValues;
            return _aliasService.Lookup(rvd).FirstOrDefault();
        }

        public IEnumerable<string> GetAliases(ContentItem content)
        {
            var rvd = content.ContentManager.GetItemMetadata(content).DisplayRouteValues;
            return _aliasService.Lookup(rvd);
        }
    }
}