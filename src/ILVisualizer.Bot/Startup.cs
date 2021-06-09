using System.Threading.Tasks;
using ILVisualizer.Bot.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ILVisualizer.Bot
{
	public class Startup
	{
		static async Task Main()
		{
			IHost host = Host.CreateDefaultBuilder()
				.ConfigureHost()
				.UseConsoleLifetime()
				.Build();

			await host.RunAsync().ConfigureAwait(false);
		}
	}
}