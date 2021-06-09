using ILVisualizer.Application.Common.ConfigOptions;

namespace ILVisualizer.Application.Common.Interfaces
{
	public interface IConfig
	{
		DiscordConfigOptions DiscordConfig { get; init; }
		EnvironmentConfigOptions EnvironmentConfig { get; init; }
	}
}