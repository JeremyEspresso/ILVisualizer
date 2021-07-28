using System;
using ILVisualizer.Application.Common.Config.Options;
using ILVisualizer.Application.Common.Interfaces;

namespace ILVisualizer.Application.Common.Config
{
	public class Config : IConfig
	{
		private Config() { }

		public DiscordConfigOptions DiscordConfig { get; init; }
		public EnvironmentConfigOptions EnvironmentConfig { get; init; }

		public static IConfig Get() =>
			new Config
			{
				DiscordConfig = new DiscordConfigOptions
				{
					Token = Environment.GetEnvironmentVariable(ApplicationConstants.DiscordTokenEnvironmentVariable),
					Prefix = Environment.GetEnvironmentVariable(ApplicationConstants.DiscordPrefixEnvironmentVariable),
				},
				EnvironmentConfig = new EnvironmentConfigOptions
				{
					ApplicationName = Environment.GetEnvironmentVariable(ApplicationConstants.ApplicationNameEnvironmentVariable),
					EnvironmentName = Environment.GetEnvironmentVariable(ApplicationConstants.EnvironmentNameEnvironmentVariable),
				}
			};
	}
}