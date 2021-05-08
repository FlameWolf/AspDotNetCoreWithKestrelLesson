using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspDotNetCoreWithKestrelLesson.Filters
{
	public class ExceptionFilter : IAsyncExceptionFilter
	{
		public async Task OnExceptionAsync(ExceptionContext context)
		{
			context.Result = new ObjectResult(context.Exception)
			{
				StatusCode = StatusCodes.Status500InternalServerError
			};
			context.ExceptionHandled = true;
		}
	}
}