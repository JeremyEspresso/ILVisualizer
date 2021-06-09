using System;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using ILVisualizer.Application.Common.Interfaces;
using Microsoft.Extensions.Hosting;

namespace ILVisualizer.Bot
{
	public class ILVisualizerBot : IHostedService
	{
		private readonly DiscordClient _discord;
		private readonly IConfig _config;

		public ILVisualizerBot(DiscordClient discordClient, IConfig config)
		{
			_discord = discordClient;
			_config = config;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await _discord.InitializeAsync();
			await _discord.ConnectAsync();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}
	}
}