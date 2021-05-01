using System;
using System.Collections.Generic;

namespace AspDotNetCoreWithKestrelLesson.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class GenerateExampleAttribute: Attribute
	{
		public List<object> ExampleValues = new();

		public GenerateExampleAttribute(params object[] exampleValues)
		{
			ExampleValues.AddRange(exampleValues);
		}
	}
}