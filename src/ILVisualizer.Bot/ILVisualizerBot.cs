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

namespace ILVisualizer.Bot
{
	public class ILVisualizerBot : BackgroundService
	{
		private readonly DiscordClient _discord;
		private readonly IConfig _config;
		private readonly IServiceProvider _serviceProvider;
		private ICommandContextFactory _commandContextFactory;
		private ICommandExecutor _commandExecutor;
		private ICommandParser _commandParser;
		private ICommandStore _commandStore;

		public ILVisualizerBot(
			DiscordClient discordClient,
			IConfig config,
			IServiceProvider serviceProvider)
		{
			_discord = discordClient;
			_config = config;
			_serviceProvider = serviceProvider;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			using IServiceScope scope = _serviceProvider.CreateScope();

			_commandContextFactory = scope.ServiceProvider.GetRequiredService<ICommandContextFactory>();
			_commandExecutor = scope.ServiceProvider.GetRequiredService<ICommandExecutor>();
			_commandParser = scope.ServiceProvider.GetRequiredService<ICommandParser>();
			_commandStore = scope.ServiceProvider.GetRequiredService<ICommandStore>();

			SubscribeEvents();

			await _discord.InitializeAsync();
			await _discord.ConnectAsync();
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
			catch (Exception ex)
			{
				// If it's not a valid command we really don't care :P
				return;
			}

			context.Items.Add(ILCmdContext.Ctx, new ILCmdContext(e.Message));

			await _commandExecutor.ExecuteAsync(context);
		}
	}
}