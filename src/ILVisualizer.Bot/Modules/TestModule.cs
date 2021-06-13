using System.Threading.Tasks;
using DSharpPlus;
using Finite.Commands;
using Finite.Commands.AttributedModel;
using ILVisualizer.Application.Common.Entities.DiscordCommands;
using Microsoft.Extensions.Logging;

namespace ILVisualizer.Bot.Modules
{
	public class TestModule : Module
	{
		private readonly DiscordClient _client;
		private readonly ILogger<TestModule> _logger;

		public TestModule(DiscordClient client, ILogger<TestModule> logger)
		{
			_client = client;
			_logger = logger;
		}

		[Command("+ping")]
		public async ValueTask<ICommandResult> PingPongCommand()
		{
			ILCmdContext commandContext = (Context.Items[ILCmdContext.Ctx] as ILCmdContext)!;
			await commandContext!.Channel.SendMessageAsync($"pong!\n**WebSocket Latency:** `{_client.Ping}`ms");
			_logger.LogDebug("ping command executed");
			return await new ValueTask<ICommandResult>(new NoContentCommandResult());
		}
	}
}