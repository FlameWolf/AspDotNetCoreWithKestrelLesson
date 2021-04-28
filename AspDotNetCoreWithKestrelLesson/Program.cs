using AspDotNetCoreWithKestrelLesson;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

static IHostBuilder CreateHostBuilder(string[] args)
{
	return Host
		.CreateDefaultBuilder(args)
		.UseServiceProviderFactory(new AutofacServiceProviderFactory())
		.ConfigureWebHostDefaults(webBuilder =>
		{
			webBuilder.UseStartup<Startup>();
			webBuilder.ConfigureLogging(config =>
			{
				config.AddConsole();
				config.AddDebug();
				config.AddEventLog();
			});
			webBuilder.UseKestrel();
		});
}

CreateHostBuilder(args).Build().Run();