using Finite.Commands;
using Finite.Commands.Parsing;
using ILVisualizer.Application.Common;
using ILVisualizer.Application.Common.Interfaces;
using ILVisualizer.Bot.Extensions;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ILVisualizer.Bot.Responders
{
	public class MessageCreatedResponder : IResponder<IMessageCreate>
	{
		private readonly ILogger<MessageCreatedResponder> _logger;
		private readonly ICommandContextFactory _commandContextFactory;
		private readonly ICommandExecutor _commandExecutor;
		private readonly ICommandParser _commandParser;

		private readonly char _prefix;

		public MessageCreatedResponder(ILogger<MessageCreatedResponder> logger, ICommandContextFactory commandContextFactory, ICommandExecutor commandExecutor, ICommandParser commandParser, IConfig config)
		{
			_logger = logger;
			_commandContextFactory = commandContextFactory;
			_commandExecutor = commandExecutor;
			_commandParser = commandParser;
			_prefix = Convert.ToChar(config.DiscordConfig.Prefix);
		}

		public async Task<Result> RespondAsync(IMessageCreate gatewayEvent, CancellationToken ct = new())
		{
			if ((gatewayEvent.Author.IsBot.HasValue && gatewayEvent.Author.IsBot.Value) 
			    || gatewayEvent.Embeds.Count > 1
			    || !gatewayEvent.TrySlicePrefix(_prefix, out var commandContent))
			{
				return Result.FromSuccess();
			}

			var context = _commandContextFactory.CreateContext();

			try
			{
				_commandParser.Parse(context, commandContent);
			}
			catch
			{
				// If it's not a valid command we really don't care :P
				return Result.FromSuccess();
			}
			
			context.Items.Add(CommandConstants.Ctx, gatewayEvent);
			
			await _commandExecutor.ExecuteAsync(context, ct);
			return Result.FromSuccess();
		}
	}
}