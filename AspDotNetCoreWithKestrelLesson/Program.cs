IHostBuilder CreateHostBuilder(string[] args)
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