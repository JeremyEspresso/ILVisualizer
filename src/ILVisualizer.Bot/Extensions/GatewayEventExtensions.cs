﻿using Remora.Discord.API.Abstractions.Gateway.Events;

namespace ILVisualizer.Bot.Extensions
{
	public static class GatewayEventExtensions
	{
		public static bool TrySlicePrefix(this IMessageCreate gatewayEvent, char prefix, out string commandContent)
		{
			if (gatewayEvent.Content[0] == prefix)
			{
				commandContent = gatewayEvent.Content[1..];
				return true;
			}

			commandContent = string.Empty;
			return false;
		}		
	}
}
