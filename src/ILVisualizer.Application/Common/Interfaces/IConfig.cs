using ILVisualizer.Application.Common.Config.Options;

namespace ILVisualizer.Application.Common.Interfaces
{
	public interface IConfig
	{
		DiscordConfigOptions DiscordConfig { get; init; }
		EnvironmentConfigOptions EnvironmentConfig { get; init; }
	}
}