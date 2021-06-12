using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Finite.Commands;
using Finite.Commands.Parsing;
using ILVisualizer.Application.Common;
using ILVisualizer.Application.Common.Entities;
using ILVisualizer.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ILVisualizer.Bot
{
	public class ILVisualizerBot : BackgroundService
	{
		private readonly DiscordClient _discord;
		private readonly IConfig _config;
		private readonly ILogger<ILVisualizerBot> _logger;
		private readonly IServiceProvider _serviceProvider;
		private ICommandContextFactory _commandContextFactory = null!;
		private ICommandExecutor _commandExecutor = null!;
		private ICommandParser _commandParser = null!;
		private ICommandStore _commandStore = null!;

		public ILVisualizerBot(
			DiscordClient discordClient,
			IConfig config,
			ILogger<ILVisualizerBot> logger,
			IServiceProvider serviceProvider)
		{
			_discord = discordClient;
			_config = config;
			_logger = logger;
			_serviceProvider = serviceProvider;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Bot started - {StartupTime}", DateTime.Now);
			using IServiceScope scope = _serviceProvider.CreateScope();

			_commandContextFactory = scope.ServiceProvider.GetRequiredService<ICommandContextFactory>();
			_commandExecutor = scope.ServiceProvider.GetRequiredService<ICommandExecutor>();
			_commandParser = scope.ServiceProvider.GetRequiredService<ICommandParser>();
			_commandStore = scope.ServiceProvider.GetRequiredService<ICommandStore>();

			SubscribeEvents();

			await _discord.InitializeAsync();
			await _discord.ConnectAsync();
			_logger.LogInformation("Discord initialized");
		}

		private void SubscribeEvents()
		{
			_discord.MessageCreated += async (_, e) => await HandleCommand(e);
		}

		private async Task HandleCommand(MessageCreateEventArgs e)
		{
			if (e.Author.IsBot) return;

			var context = _commandContextFactory.CreateContext();

			try
			{
				_commandParser.Parse(context, e.Message.Content);
			}
			catch
			{
				// If it's not a valid command we really don't care :P
				return;
			}

			context.Items.Add(ILCmdContext.Ctx, new ILCmdContext(e.Message));

			await _commandExecutor.ExecuteAsync(context);
		}
	}
}