using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Finite.Commands;
using Finite.Commands.AttributedModel;
using ILVisualizer.Application.Common;
using ILVisualizer.Application.Common.Entities;
using ILVisualizer.Application.Common.Entities.DiscordCommands;
using Microsoft.Extensions.Logging;

namespace ILVisualizer.Bot.Modules
{
	[Group("+dbg")]
	public class TestModule : Module
	{
		private readonly ILogger<TestModule> _logger;

		public TestModule(ILogger<TestModule> logger)
		{
			_logger = logger;
		}

		[Command("ping")]
		public async ValueTask<ICommandResult> PingPongCommand([Remainder] string a)
		{
			ILCmdContext commandContext = (Context.Items[ILCmdContext.Ctx] as ILCmdContext)!;
				
			await commandContext!.Channel.SendMessageAsync($"pong - {a}");
			_logger.LogDebug("ping command executed");
			return await new ValueTask<ICommandResult>(new NoContentCommandResult());
		}
	}
}