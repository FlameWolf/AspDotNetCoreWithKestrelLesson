using AspDotNetCoreWithKestrelLesson.Conventions;
using AspDotNetCoreWithKestrelLesson.Database;
using AspDotNetCoreWithKestrelLesson.Middleware;
using AspDotNetCoreWithKestrelLesson.Models;
using AspDotNetCoreWithKestrelLesson.Providers;
using AspDotNetCoreWithKestrelLesson.Repositories;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(context =>
			{
				context.UseInMemoryDatabase("AspDotNetCoreWithKestrelLessonDb");
			});
			services.AddSingleton(typeof(RecyclableMemoryStreamManager));
			services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepositoryBase<>));
			services.AddTransient(typeof(IExamplesProvider<>), typeof(PatchRequestExample<>));
			services.TryAddEnumerable
			(
				ServiceDescriptor.Transient<IApplicationModelProvider, EntityControllerModelProvider>()
			);
			services.AddControllers(options =>
			{
				options.Conventions.Add(new EntityControllerRouteConvention());
				options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
			})
			.ConfigureApplicationPartManager(config =>
			{
				config.FeatureProviders.Add(new EntityControllerFeatureProvider());
			})
			.AddNewtonsoftJson();
			services.AddSwaggerGen(config =>
			{
				config.SwaggerDoc
				(
					"v1",
					new OpenApiInfo
					{
						Title = "AspDotNetCoreWithKestrelLesson",
						Version = "v1"
					}
				);
				config.ExampleFilters();
			})
			.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
		}

		// ConfigureContainer is where you can register things directly with Autofac.
		// This runs after ConfigureServices so the things here will override registrations made in ConfigureServices.
		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder
				.RegisterGeneric(typeof(PatchRequestExample<>))
				.As(typeof(IExamplesProvider<>))
				.InstancePerDependency();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint
					(
						"/swagger/v1/swagger.json",
						"AspDotNetCoreWithKestrelLesson v1"
					);
				});
			}
			app.UseRequestResponseLogging();
			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}