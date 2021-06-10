using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Finite.Commands;
using Finite.Commands.AttributedModel;
using ILVisualizer.Application.Common;
using ILVisualizer.Application.Common.Entities;

namespace ILVisualizer.Bot.Modules
{
	[Group("+dbg")]
	public class TestModule : Module
	{
		[Command("ping")]
		public async ValueTask<ICommandResult> PingPongCommand([Remainder] string a)
		{
			ILCmdContext commandContext = (Context.Items[ILCmdContext.Ctx] as ILCmdContext)!;
			
			await commandContext!.Channel.SendMessageAsync($"pong - {a}");
			
			return await new ValueTask<ICommandResult>(new NoContentCommandResult());
		}
	}
}