using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspDotNetCoreWithKestrelLesson.Database;
using AspDotNetCoreWithKestrelLesson.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace AspDotNetCoreWithKestrelLesson.Middleware
{
	public class RequestResponseLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;
		private readonly RecyclableMemoryStreamManager _streamManager;
		private readonly DbContext _dbContext;
		private LogEntry _logEntry;

		public RequestResponseLoggingMiddleware
		(
			RequestDelegate next,
			ILoggerFactory loggerFactory,
			RecyclableMemoryStreamManager streamManager,
			IServiceProvider serviceProvider
		)
		{
			_next = next;
			_logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
			_streamManager = streamManager;
			_dbContext = serviceProvider.GetService<ApplicationDbContext>();
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
			_logEntry.Scheme = context.Request.Scheme;
			_logEntry.Host = context.Request.Host.Value;
			_logEntry.User = context.User.Identity.Name;
			_logEntry.Claims = context
				.User
				.Claims
				.Aggregate<Claim, string>
				(
					string.Empty, (x, y) => $"{x}; ${y.Type} = ${y.Value}"
				);
			_logEntry.Path = context.Request.Path;
			_logEntry.QueryString = context.Request.QueryString.Value;
			_logEntry.Request = ReadStreamInChunks(requestStream);
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
			_logEntry.Response = responseText;
			await responseStream.CopyToAsync(originalResponseBody);
		}

		public async Task Invoke(HttpContext httpContext)
		{
			_logEntry = new();
			var responseStream = await LogRequest(httpContext);
			_logEntry.StartedAt = DateTime.Now;
			await _next(httpContext);
			_logEntry.EndedAt = DateTime.Now;
			await LogResponse(httpContext, responseStream);
			var updateResult = _dbContext.Update(_logEntry);
			await _dbContext.SaveChangesAsync();
			_logger.LogInformation(updateResult.Entity.ToString());
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