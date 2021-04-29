using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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