using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace ILVisualizer.Bot.Modules
{
	public class TestModule : BaseCommandModule
	{
		[Command("ping")]
		public Task PingPongCommand(CommandContext ctx)
		{
			return ctx.RespondAsync("pong");
		}
	}
}