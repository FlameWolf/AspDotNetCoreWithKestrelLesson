using AspDotNetCoreWithKestrelLesson.Attributes;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AspDotNetCoreWithKestrelLesson.Models
{
	public class RequestExample<T> : IExamplesProvider<T>
	{
		public T GetExamples()
		{
			var generateExampleAttribute = typeof(T).GetCustomAttribute<GenerateExampleAttribute>();
			return
				generateExampleAttribute == null ?
				default(T) :
				(T)Activator.CreateInstance
				(
					typeof(T),
					generateExampleAttribute.ExampleValues.ToArray()
				);
		}
	}

	public class PatchRequestExample<T> : IExamplesProvider<PatchRequest<T>>
	{
		public PatchRequest<T> GetExamples()
		{
			return new PatchRequest<T>
			(
				new RequestExample<T>().GetExamples()
			);
		}
	}
}