using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;
using Orchard.Mvc;

namespace AIM.ActionAlternates.Services
{
	[OrchardFeature("AIM.DesignerTools.ActionAlternates")]
	public class ActionAlternatesFactory : ShapeDisplayEvents
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly Lazy<List<string>> _actionAlternates;

		public ActionAlternatesFactory(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;

			_actionAlternates = new Lazy<List<string>>(() =>
			{
				var httpContext = _httpContextAccessor.Current();

				if (httpContext == null)
				{
					return null;
				}

				var request = _httpContextAccessor.Current().Request;

				var actionSegments = new[] 
				{
					request.RequestContext.RouteData.GetRequiredString("area").Replace("-", "__").Replace(".", "_"), 
					request.RequestContext.RouteData.GetRequiredString("controller").Replace("-", "__").Replace(".", "_"), 
					request.RequestContext.RouteData.GetRequiredString("action").Replace("-", "__").Replace(".", "_")
				};

				return Enumerable.Range(1, actionSegments.Count()).Select(range => String.Join("__", actionSegments.Take(range))).ToList();
			});
		}

		public override void Displaying(ShapeDisplayingContext context)
		{
			context.ShapeMetadata.OnDisplaying(displayedContext =>
			{
				if (_actionAlternates.Value == null || !_actionAlternates.Value.Any())
				{
					return;
				}

				// prevent applying alternate again, c.f. http://orchard.codeplex.com/workitem/18298
				if (displayedContext.ShapeMetadata.Alternates.Any(x => x.Contains("__action__")))
				{
					return;
				}

				// appends Url alternates to current ones
				displayedContext.ShapeMetadata.Alternates = displayedContext.ShapeMetadata.Alternates.SelectMany(
					alternate => new[] { alternate }.Union(_actionAlternates.Value.Select(a => alternate + "__action__" + a))
					).ToList();

				// appends [ShapeType]__action__[Url] alternates
				displayedContext.ShapeMetadata.Alternates = _actionAlternates.Value.Select(altPath => displayedContext.ShapeMetadata.Type + "__action__" + altPath)
					.Union(displayedContext.ShapeMetadata.Alternates)
					.ToList();
			});
		}
	}
}
