using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspDotNetCoreWithKestrelLesson.Filters
{
	public class ExceptionFilter : IAsyncExceptionFilter
	{
		public async Task OnExceptionAsync(ExceptionContext context)
		{
			context.Result = new ObjectResult
			(
				new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.InternalServerError,
					ReasonPhrase = "An unexpected error occurred",
					Content = new StringContent(context.Exception.ToString())
				}
			);
			context.ExceptionHandled = true;
		}
	}
}