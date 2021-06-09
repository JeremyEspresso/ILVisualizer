using DSharpPlus;
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

			builder.ConfigureServices((_, services) =>
			{
				services.AddSingleton(new DiscordClient(new DiscordConfiguration
				{
					Token = applicationConfig.DiscordConfig.Token,
					TokenType = TokenType.Bot,
					Intents = DiscordIntents.GuildMessages | DiscordIntents.Guilds,
				}));

				services.AddSingleton(applicationConfig);
				services.AddHostedService<ILVisualizerBot>();
				services.AddApplication();
			});

			return builder;
		}
	}
}