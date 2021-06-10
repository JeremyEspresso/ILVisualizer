using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using ILVisualizer.Application.Common.Extensions;
using ILVisualizer.Application.Common.Interfaces;

namespace ILVisualizer.Application.Services
{
	public class CommandHandlerService : ICommandHandlerService
	{
		private readonly DiscordClient _discord;
		private readonly string _prefix;

		public CommandHandlerService(DiscordClient discord, IConfig config)
		{
			_discord = discord;
			_prefix = config.DiscordConfig.Prefix;
		}

		public Task HandleCommands(DiscordClient sender, MessageCreateEventArgs e)
		{
			if (e.Author.IsBot || string.IsNullOrEmpty(e.Message.Content))
				return Task.CompletedTask;
			if (!e.Message.TryGetStringPrefixLength(_prefix, out int length))
				return Task.CompletedTask;

			var cnext = _discord.GetCommandsNext();

			var cmd = e.Message.Content[length..];
			var commandActual = cnext.FindCommand(cmd, out var argsRaw);

			if (commandActual is null)
				return Task.CompletedTask;

			var context = cnext.CreateContext(e.Message, _prefix, commandActual, argsRaw);

			ThreadPool.QueueUserWorkItem(async (_) => await cnext.ExecuteCommandAsync(context));
			return Task.CompletedTask;
		}
	}
}