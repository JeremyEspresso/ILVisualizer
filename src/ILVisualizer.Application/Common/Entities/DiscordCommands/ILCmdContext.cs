using DSharpPlus.Entities;

namespace ILVisualizer.Application.Common.Entities.DiscordCommands
{
	public class ILCmdContext
	{
		public const string Ctx = "ctx";

		public ILCmdContext(DiscordMessage message) => Message = message;

		public DiscordMessage Message { get; }

		public DiscordChannel Channel => Message.Channel;

		public DiscordGuild Guild => Channel.Guild;

		public DiscordUser User => Message.Author;
	}
}