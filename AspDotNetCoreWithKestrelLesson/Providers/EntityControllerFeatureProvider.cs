using AspDotNetCoreWithKestrelLesson.Attributes;
using AspDotNetCoreWithKestrelLesson.Controllers;
using AspDotNetCoreWithKestrelLesson.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AspDotNetCoreWithKestrelLesson.Providers
{
	public class EntityControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
	{
		public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
		{
			typeof(EntityControllerFeatureProvider)
				.Assembly
				.GetExportedTypes()
				.Where
				(
					x => x.GetCustomAttribute<GenerateControllerAttribute>() != null
				)
				.ForEach
				(
					x => feature.Controllers.Add
					(
						typeof(EntityControllerBase<>).MakeGenericType(x).GetTypeInfo()
					)
				);
		}
	}
}