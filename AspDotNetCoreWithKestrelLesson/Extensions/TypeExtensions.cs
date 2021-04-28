using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreWithKestrelLesson.Extensions
{
	public static class TypeExtensions
	{
		private static ConcurrentDictionary<Type, object> typeDefaults = new();

		public static object GetDefaultValue(this Type type) =>
			type.IsValueType ?
			typeDefaults.GetOrAdd(type, Activator.CreateInstance) :
			type.GetConstructor(Type.EmptyTypes)?.Invoke(Array.Empty<object>());
	}
}