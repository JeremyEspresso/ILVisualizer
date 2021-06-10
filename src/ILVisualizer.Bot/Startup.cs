using System.Threading.Tasks;
using ILVisualizer.Bot.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ILVisualizer.Bot
{
	public class Startup
	{
		static void Main()
			=> Host.CreateDefaultBuilder()
				.ConfigureHost()
				.Build()
				.Run();
	}
}