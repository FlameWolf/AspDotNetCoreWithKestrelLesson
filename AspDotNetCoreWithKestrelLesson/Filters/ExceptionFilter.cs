namespace AspDotNetCoreWithKestrelLesson.Filters;

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