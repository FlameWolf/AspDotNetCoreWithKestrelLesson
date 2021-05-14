using System;
using System.Collections.Generic;

namespace AspDotNetCoreWithKestrelLesson.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class GenerateExampleAttribute: Attribute
	{
		public IEnumerable<object> ExampleValues { init; get; }

		public GenerateExampleAttribute(params object[] exampleValues)
		{
			ExampleValues = exampleValues;
		}
	}
}