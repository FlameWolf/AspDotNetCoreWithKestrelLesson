using System.Linq;
using AspDotNetCoreWithKestrelLesson.Controllers;
using AspDotNetCoreWithKestrelLesson.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AspDotNetCoreWithKestrelLesson.Providers
{
	public class EntityControllerModelProvider : IApplicationModelProvider
	{
		public int Order => -991;

		public void OnProvidersExecuting(ApplicationModelProviderContext context)
		{
			context
				.Result
				.Controllers
				.Where
				(
					x => x.ControllerType.IsGenericType &&
						(
							x.ControllerType.GetGenericTypeDefinition() ==
							typeof(EntityControllerBase<>)
						)
				)
				.ForEach
				(
					x =>
					x.ControllerName = x.ControllerType.GenericTypeArguments.First().Name
				);
		}

		public void OnProvidersExecuted(ApplicationModelProviderContext context)
		{
		}
	}
}