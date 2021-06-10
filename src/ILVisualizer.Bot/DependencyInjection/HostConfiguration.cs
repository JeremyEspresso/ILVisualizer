using System;
using DSharpPlus;
using Finite.Commands;
using Finite.Commands.Parsing;
using ILVisualizer.Application.Common;
using ILVisualizer.Application.Common.Interfaces;
using ILVisualizer.Application.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

			builder.ConfigureServices((context, services) =>
			{
				_ = services.AddSingleton(new DiscordClient(new DiscordConfiguration
				{
					Token = applicationConfig.DiscordConfig.Token,
					TokenType = TokenType.Bot,
					Intents = DiscordIntents.GuildMessages | DiscordIntents.Guilds,
				}));

				_ = services.AddSingleton(applicationConfig);
				_ = services.AddApplication();

				_ = services.Configure<HostOptions>(x => x.ShutdownTimeout = TimeSpan.Zero);
				_ = services.AddCommands()
					.AddPositionalCommandParser()
					.AddAttributedCommands(x
						=> x.Assemblies.Add(typeof(Startup).Assembly.Location));

				_ = services.AddHostedService<ILVisualizerBot>();
			});

			
			return builder;
		}
	}
}