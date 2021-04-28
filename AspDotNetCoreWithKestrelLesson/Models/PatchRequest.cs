using AspDotNetCoreWithKestrelLesson.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AspDotNetCoreWithKestrelLesson.Models
{
	public partial record PatchRequest<TRequest>
	{
		public PatchRequest() : this("test", string.Empty, string.Empty, string.Empty)
		{
			var requestAsJObject = GetInstanceAsJObject<TRequest>();
			Path = requestAsJObject.Properties().FirstOrDefault().Name;
			Value = requestAsJObject.Properties().FirstOrDefault().Value.ToString();
		}

		public PatchRequest(TRequest request) : this()
		{
			var requestAsJObject = ConvertToJObject(request);
			Path = requestAsJObject.Properties().FirstOrDefault().Name;
			Value = requestAsJObject.Properties().FirstOrDefault().Value.ToString();
		}

		public static implicit operator JsonPatchDocument(PatchRequest<TRequest> request)
		{
			var patchDocument = new JsonPatchDocument();
			patchDocument.Operations.Add
			(
				new Operation
				{
					op = request.Op,
					from = request.From,
					path = request.Path,
					value = request.Value
				}
			);
			return patchDocument;
		}

		private JObject ConvertToJObject(object source)
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

		private JObject GetInstanceAsJObject<TSource>(params object[] args)
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
				constructor.Invoke
				(
					parameters.Select
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