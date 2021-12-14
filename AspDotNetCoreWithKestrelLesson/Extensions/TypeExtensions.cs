namespace AspDotNetCoreWithKestrelLesson.Extensions;

public static class TypeExtensions
{
	private static readonly ConcurrentDictionary<Type, object> typeDefaults = new();

	public static object GetDefaultValue(this Type type) =>
		type.IsValueType ?
		typeDefaults.GetOrAdd(type, Activator.CreateInstance) :
		type.GetConstructor(Type.EmptyTypes)?.Invoke(Array.Empty<object>());
}