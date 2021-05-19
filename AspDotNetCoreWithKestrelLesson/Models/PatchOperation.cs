using System;
using System.Linq;
using System.Reflection;
using AspDotNetCoreWithKestrelLesson.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace AspDotNetCoreWithKestrelLesson.Models
{
	public partial record PatchOperation<TRequest>
	{
		private JObject Initialiser
		{
			init
			{
				Path = value.Properties().FirstOrDefault().Name;
				Value = value.Properties().FirstOrDefault().Value.ToString();
			}
		}

		public PatchOperation() : this("test", string.Empty, string.Empty, string.Empty)
		{
			Initialiser = GetInstanceAsJObject<TRequest>();
		}

		public PatchOperation(TRequest request) : this()
		{
			Initialiser = ConvertToJObject(request);
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