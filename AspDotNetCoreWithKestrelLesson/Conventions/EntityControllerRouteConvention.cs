namespace AspDotNetCoreWithKestrelLesson.Conventions;

public class EntityControllerRouteConvention : IControllerModelConvention
{
	public void Apply(ControllerModel controller)
	{
		if (controller.ControllerType.IsGenericType)
		{
			var genericTypeArgument = controller.ControllerType.GenericTypeArguments[0];
			var generatedControllerAttribute = genericTypeArgument.GetCustomAttribute<GenerateControllerAttribute>();
			string route = string.IsNullOrEmpty(generatedControllerAttribute?.Route) ?
				genericTypeArgument.Name.ToCamel() :
				generatedControllerAttribute.Route;
			controller.Selectors.Add
			(
				new SelectorModel
				{
					AttributeRouteModel = new AttributeRouteModel
					(
						new RouteAttribute(route)
					)
				}
			);
		}
	}
}