using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace ILVisualizer.Application.Common.Extensions
{
	public static class DiscordMessageExtensions
	{
		public static bool TryGetStringPrefixLength(this DiscordMessage msg, string prefix, out int length)
		{
			length = msg.GetStringPrefixLength(prefix);
			return length != -1;
		}
	}
}