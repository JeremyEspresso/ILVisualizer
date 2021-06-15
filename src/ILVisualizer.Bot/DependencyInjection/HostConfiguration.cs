using ILVisualizer.Application.Common.Config;
using ILVisualizer.Application.Common.Interfaces;
using ILVisualizer.Application.DependencyInjection;
using System;
using Finite.Commands;
using Finite.Commands.Parsing;
using ILVisualizer.Application.Common.Services;
using ILVisualizer.Bot.Responders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Remora.Discord.Gateway.Extensions;

namespace ILVisualizer.Bot.DependencyInjection
{
	public static class HostConfiguration
	{
		public static IHostBuilder ConfigureHost(this IHostBuilder builder)
		{
			IConfig applicationConfig = Config.Get();
			
			builder.ConfigureAppConfiguration((context, _) =>
			{
				context.HostingEnvironment.ApplicationName = applicationConfig.EnvironmentConfig.ApplicationName;
				context.HostingEnvironment.EnvironmentName = applicationConfig.EnvironmentConfig.EnvironmentName;
			});

			builder.ConfigureServices((_, services) =>
			{
				services.AddDiscordGateway(_ => applicationConfig.DiscordConfig.Token);
				services.AddResponder<MessageCreatedResponder>();
				services.AddResponder<InteractionCreatedResponder>();

				services.AddSingleton(applicationConfig);
				services.AddApplication();

				services.AddScoped<IILParserService, ILParserService>();
				services.AddScoped<IILProcessorService, ILProcessorService>();

				services.Configure<HostOptions>(x => x.ShutdownTimeout = TimeSpan.Zero);
				services.AddCommands()
					.AddPositionalCommandParser()
					.AddAttributedCommands(x
						=> x.Assemblies.Add(typeof(Startup).Assembly.Location));

				services.AddHttpClient();
				services.AddHostedService<ILVisualizerBot>();
			});

			
			return builder;
		}
	}
}