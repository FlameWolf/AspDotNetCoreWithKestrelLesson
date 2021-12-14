namespace AspDotNetCoreWithKestrelLesson.Providers;

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