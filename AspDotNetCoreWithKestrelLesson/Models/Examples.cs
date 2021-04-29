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
			var exampleAttribute = typeof(T).GetCustomAttribute<GenerateExampleAttribute>();
			return (T)
			(
				Activator.CreateInstance
				(
					typeof(T),
					exampleAttribute?.ExampleValues.ToArray()
				)
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