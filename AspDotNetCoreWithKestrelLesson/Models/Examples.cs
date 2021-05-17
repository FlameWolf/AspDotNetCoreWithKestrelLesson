using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AspDotNetCoreWithKestrelLesson.Attributes;
using Swashbuckle.AspNetCore.Filters;

namespace AspDotNetCoreWithKestrelLesson.Models
{
	public class RequestExample<T> : IExamplesProvider<T>
	{
		public T GetExamples()
		{
			T instance = (T)RuntimeHelpers.GetUninitializedObject(typeof(T));
			var generateExampleAttribute = typeof(T).GetCustomAttribute<GenerateExampleAttribute>();
			if (generateExampleAttribute?.Values.Length > 0)
			{
				if (generateExampleAttribute.Properties.Length == 0)
				{
					return (T)Activator.CreateInstance
					(
						typeof(T),
						generateExampleAttribute.Values.ToArray()
					);
				}
				else
				{
					var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance);
					foreach
					(
						var (propName, index)
						in generateExampleAttribute
							.Properties
							.Select((item, index) => (item, index))
					)
					{
						props
							.First(x => x.Name == propName)?
							.SetValue
							(
								instance,
								generateExampleAttribute.Values[index]
							);
					}
				}
			}
			return instance;
		}
	}

	public class PatchRequestExample<T> : IExamplesProvider<PatchRequest<T>>
	{
		public PatchRequest<T> GetExamples()
		{
			return new PatchRequest<T>
			{
				new PatchOperation<T>
				(
					new RequestExample<T>().GetExamples()
				)
			};
		}
	}
}