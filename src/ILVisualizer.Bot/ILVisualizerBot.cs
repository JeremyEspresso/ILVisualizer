using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Remora.Discord.Gateway;

namespace ILVisualizer.Bot
{
	public class ILVisualizerBot : BackgroundService
	{
		private readonly DiscordGatewayClient _discordGatewayClient;
		private readonly ILogger<ILVisualizerBot> _logger;

		public ILVisualizerBot(DiscordGatewayClient discordGatewayClient, ILogger<ILVisualizerBot> logger)
		{
			_discordGatewayClient = discordGatewayClient;
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Bot started - {StartupTime}", DateTime.Now);
			await _discordGatewayClient.RunAsync(stoppingToken);
		}
	}
}