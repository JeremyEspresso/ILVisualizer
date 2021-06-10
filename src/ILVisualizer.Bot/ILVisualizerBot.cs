using System;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using ILVisualizer.Application.Common.Interfaces;
using Microsoft.Extensions.Hosting;

namespace ILVisualizer.Bot
{
	public class ILVisualizerBot : IHostedService
	{
		private readonly DiscordClient _discord;
		private readonly IConfig _config;
		private readonly ICommandHandlerService _commandHandlerService;

		public ILVisualizerBot(DiscordClient discordClient, IConfig config, ICommandHandlerService commandHandlerService)
		{
			_discord = discordClient;
			_config = config;
			_commandHandlerService = commandHandlerService;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			RegisterCommandHandler();
			ConfigureCommandsNext();

			await _discord.InitializeAsync();
			await _discord.ConnectAsync();
			
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await _discord.DisconnectAsync();
			_discord.Dispose();
		}

		private void ConfigureCommandsNext()
		{
			_discord.UseCommandsNext(new CommandsNextConfiguration
			{
				CaseSensitive = false,
				IgnoreExtraArguments = true,
				UseDefaultCommandHandler = false,
			});

			var commandsNext = _discord.GetCommandsNext();
			commandsNext.RegisterCommands(typeof(ILVisualizerBot).Assembly);
		}

		private void RegisterCommandHandler()
		{
			_discord.MessageCreated += async (c, e) =>
			{
				await _commandHandlerService.HandleCommands(c, e);
			};
		}
	}
}