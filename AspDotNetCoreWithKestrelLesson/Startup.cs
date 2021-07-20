using System;
using System.Linq;
using System.Reflection;
using AspDotNetCoreWithKestrelLesson.Conventions;
using AspDotNetCoreWithKestrelLesson.Database;
using AspDotNetCoreWithKestrelLesson.Filters;
using AspDotNetCoreWithKestrelLesson.Middleware;
using AspDotNetCoreWithKestrelLesson.Providers;
using AspDotNetCoreWithKestrelLesson.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IO;
using Swashbuckle.AspNetCore.Filters;

namespace AspDotNetCoreWithKestrelLesson
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
		{
			return new ServiceCollection()
				.AddLogging()
				.AddControllers()
				.AddNewtonsoftJson()
				.Services
				.BuildServiceProvider()
				.GetRequiredService<IOptions<MvcOptions>>()
				.Value
				.InputFormatters
				.OfType<NewtonsoftJsonPatchInputFormatter>()
				.First();
		}

		private RecyclableMemoryStreamManager CreateRecyclableMemoryStreamManager(int blockSize)
		{
			var largeBufferMultiple = (blockSize * blockSize);
			var maxBufferSize = (16 * largeBufferMultiple);
			var maxFreeSmallPoolBytes = (128 * blockSize);
			var maxFreeLargePoolBytes = (maxBufferSize * 4);
			return new RecyclableMemoryStreamManager(maxFreeSmallPoolBytes, maxFreeLargePoolBytes);
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(context =>
			{
				context.UseInMemoryDatabase(nameof(ApplicationDbContext));
			});
			services.AddSingleton(typeof(RecyclableMemoryStreamManager), CreateRecyclableMemoryStreamManager(1024));
			services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepositoryBase<>));
			services.TryAddEnumerable
			(
				ServiceDescriptor.Transient<IApplicationModelProvider, EntityControllerModelProvider>()
			);
			services.AddCors(options =>
			{
				options.AddDefaultPolicy
				(
					builder =>
					{
						builder.SetIsOriginAllowed
						(
							origin => new Uri(origin).IsLoopback
						);
					}
				);
			});
			services.AddControllers(options =>
			{
				options.Conventions.Add(new EntityControllerRouteConvention());
				options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
				options.Filters.Add<ExceptionFilter>();
			})
			.ConfigureApplicationPartManager(config =>
			{
				config.FeatureProviders.Add(new EntityControllerFeatureProvider());
			})
			.AddNewtonsoftJson();
			services.AddSwaggerGen(config =>
			{
				config.ExampleFilters();
			})
			.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseRequestResponseLogging();
			app.UseRouting();
			app.UseCors();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}