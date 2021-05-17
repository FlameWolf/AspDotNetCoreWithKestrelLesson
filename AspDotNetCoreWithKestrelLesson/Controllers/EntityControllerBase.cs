using System;
using System.Linq;
using System.Threading.Tasks;
using AspDotNetCoreWithKestrelLesson.Extensions;
using AspDotNetCoreWithKestrelLesson.Models;
using AspDotNetCoreWithKestrelLesson.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace AspDotNetCoreWithKestrelLesson.Controllers
{
	public class EntityControllerBase<T> : ControllerBase
	{
		private readonly IEntityRepository<T> _repository;

		public EntityControllerBase(IServiceProvider provider)
		{
			_repository = provider.GetService<IEntityRepository<T>>();
		}

		protected string GetTemplateForAction(string actionName)
		{
			var provider = HttpContext.RequestServices.GetService<IActionDescriptorCollectionProvider>();
			return provider.ActionDescriptors.Items.FirstOrDefault
			(
				x => (x as ControllerActionDescriptor)?.ActionName == actionName
			)
			.AttributeRouteInfo?.Template;
		}

		protected PropertyType GetPropertyValue<PropertyType>(string propertyName, object source)
		{
			return (PropertyType)source.GetType().GetProperty(propertyName).GetValue(source);
		}

		protected async Task<IActionResult> Find(int id, Func<T, Task<IActionResult>> responseHandler)
		{
			var response = await _repository.Get(id);
			if (response == null)
			{
				return NotFound
				(
					$"Unable to find a {typeof(T).Name.ToCamel()} with the specified ID ({id})"
				);
			}
			return await responseHandler(response);
		}

		[Route("add")]
		[HttpPost]
		public async Task<IActionResult> Add([FromBody] T request)
		{
			var id = GetPropertyValue<int>("Id", request);
			var response = await _repository.Get(id);
			if (response != null)
			{
				return Conflict
				(
					$"A {typeof(T).Name.ToCamel()} with the specified ID ({id}) already exists"
				);
			}
			response = await _repository.Add(request);
			var routeTemplate = GetTemplateForAction(nameof(Get));
			return Created
			(
				routeTemplate.Replace
				(
					"{id}",
					GetPropertyValue<int>("Id", response).ToString()
				),
				response
			);
		}

		[Route("get/{id}")]
		[HttpGet]
		public async Task<IActionResult> Get(int id)
		{
			return await Find(id, async response => Ok(response));
		}

		[Route("get")]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var response = await _repository.Get();
			return Ok(response);
		}

		[Route("update/{id}")]
		[HttpPatch]
		public async Task<IActionResult> Update(int id, [FromBody] PatchRequest<T> request)
		{
			return await Find(id, async response =>
			{
				((JsonPatchDocument)request).ApplyTo(response, error =>
				{
					ModelState.AddModelError
					(
						error.AffectedObject.GetType().Name,
						error.ErrorMessage
					);
				});
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				return Ok(await _repository.Update((T)response));
			});
		}

		[Route("delete/{id}")]
		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			return await Find
			(
				id,
				async response => Ok(await _repository.Delete((T)response))
			);
		}
	}
}