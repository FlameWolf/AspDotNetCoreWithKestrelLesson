using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreWithKestrelLesson.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class GenerateControllerAttribute : Attribute
	{
		public string Route { set; get; }

		public GenerateControllerAttribute()
		{
		}

		public GenerateControllerAttribute(string route)
		{
			Route = route;
		}
	}
}