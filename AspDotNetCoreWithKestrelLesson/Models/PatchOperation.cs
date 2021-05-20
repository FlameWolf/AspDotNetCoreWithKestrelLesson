using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace AspDotNetCoreWithKestrelLesson.Models
{
	public partial record PatchOperation<T>
	{
		private PatchOperation() : this("test", string.Empty, string.Empty, null) => Expression.Empty();

		public PatchOperation(T request) : this()
		{
			var requestAsJObject = ConvertToJObject(request);
			Path = requestAsJObject.Properties().FirstOrDefault().Name;
			Value = requestAsJObject.Properties().FirstOrDefault().Value;
		}

		private static JObject ConvertToJObject(object source)
		{
			return JObject.FromObject
			(
				source,
				new JsonSerializer
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				}
			);
		}
	}
}