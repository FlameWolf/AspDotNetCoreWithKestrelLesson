using AspDotNetCoreWithKestrelLesson.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace AspDotNetCoreWithKestrelLesson.Models
{
	public partial record PatchOperation<TRequest>
	{
		public PatchOperation() : this("test", string.Empty, string.Empty, string.Empty)
		{
			var requestAsJObject = GetInstanceAsJObject<TRequest>();
			Path = requestAsJObject.Properties().FirstOrDefault().Name;
			Value = requestAsJObject.Properties().FirstOrDefault().Value.ToString();
		}

		public PatchOperation(TRequest request) : this()
		{
			var requestAsJObject = ConvertToJObject(request);
			Path = requestAsJObject.Properties().FirstOrDefault().Name;
			Value = requestAsJObject.Properties().FirstOrDefault().Value.ToString();
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

		private static JObject GetInstanceAsJObject<TSource>()
		{
			var constructor = typeof(TSource)
				.GetConstructors
				(
					BindingFlags.Public | BindingFlags.Instance
				)
				.FirstOrDefault();
			var parameters = constructor?.GetParameters();
			return ConvertToJObject
			(
				constructor?.Invoke
				(
					parameters?.Select
					(
						x => x.HasDefaultValue ?
							x.DefaultValue :
							x.ParameterType.GetDefaultValue()
					)
					.ToArray()
				)
			);
		}
	}
}