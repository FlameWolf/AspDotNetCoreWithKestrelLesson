using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace AspDotNetCoreWithKestrelLesson.Middleware
{
	public class RequestResponseLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;
		private readonly RecyclableMemoryStreamManager _streamManager;

		public RequestResponseLoggingMiddleware
		(
			RequestDelegate next,
			ILoggerFactory loggerFactory,
			RecyclableMemoryStreamManager streamManager
		)
		{
			_next = next;
			_logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
			_streamManager = streamManager;
		}

		private static string ReadStreamInChunks(Stream stream)
		{
			const int bufferSize = 4096;
			var readChunk = new char[bufferSize];
			int readChunkSize;
			using var writer = new StringWriter();
			using var reader = new StreamReader(stream);
			stream.Seek(0, SeekOrigin.Begin);
			do
			{
				readChunkSize = reader.ReadBlock(readChunk, 0, bufferSize);
				writer.Write(readChunk, 0, readChunkSize);
			} while (readChunkSize > 0);
			return writer.ToString();
		}

		private async Task<Stream> LogRequest(HttpContext context)
		{
			context.Request.EnableBuffering();
			await using var requestStream = _streamManager.GetStream();
			await using var responseStream = context.Response.Body;
			await context.Request.Body.CopyToAsync(requestStream);
			_logger.LogInformation($"Http Request Information: {Environment.NewLine}" +
				$"Scheme: {context.Request.Scheme}{Environment.NewLine}" +
				$"Host: {context.Request.Host}{Environment.NewLine}" +
				$"Path: {context.Request.Path}{Environment.NewLine}" +
				$"Query String: {context.Request.QueryString}{Environment.NewLine}" +
				$"Request Body: {ReadStreamInChunks(requestStream)}");
			context.Request.Body.Position = 0;
			context.Response.Body = _streamManager.GetStream();
			return responseStream;
		}

		private async Task LogResponse(HttpContext context, Stream originalResponseBody)
		{
			var responseStream = context.Response.Body;
			responseStream.Seek(0, SeekOrigin.Begin);
			var responseText = await new StreamReader(responseStream).ReadToEndAsync();
			responseStream.Seek(0, SeekOrigin.Begin);
			_logger.LogInformation($"Http Response Information: {Environment.NewLine}" +
				$"Scheme: {context.Request.Scheme}{Environment.NewLine}" +
				$"Host: {context.Request.Host}{Environment.NewLine}" +
				$"Path: {context.Request.Path}{Environment.NewLine}" +
				$"Query String: {context.Request.QueryString}{Environment.NewLine}" +
				$"Response Body: {responseText}");
			await responseStream.CopyToAsync(originalResponseBody);
		}

		public async Task Invoke(HttpContext httpContext)
		{
			var responseStream = await LogRequest(httpContext);
			await _next(httpContext);
			await LogResponse(httpContext, responseStream);
		}
	}

	// Extension method used to add the middleware to the HTTP request pipeline.
	public static class RequestResponseLoggingMiddlewareExtensions
	{
		public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
		}
	}
}