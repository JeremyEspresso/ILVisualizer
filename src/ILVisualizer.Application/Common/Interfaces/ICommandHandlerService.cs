using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace ILVisualizer.Application.Common.Interfaces
{
	public interface ICommandHandlerService
	{
		Task HandleCommands(DiscordClient sender, MessageCreateEventArgs e);
	}
}