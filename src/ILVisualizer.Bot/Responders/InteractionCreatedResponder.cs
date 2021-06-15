using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Core;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using System.Threading;
using System.Threading.Tasks;

namespace ILVisualizer.Bot.Responders
{
	public class InteractionCreatedResponder : IResponder<IInteractionCreate>
	{
		private readonly IDiscordRestChannelAPI _channelAPI;
		private readonly IDiscordRestInteractionAPI _interactionAPI;
		public InteractionCreatedResponder(IDiscordRestChannelAPI channelApi, IDiscordRestInteractionAPI interactionApi)
		{
			_channelAPI = channelApi;
			_interactionAPI = interactionApi;
		}

		public async Task<Result> RespondAsync(IInteractionCreate gatewayEvent, CancellationToken ct = new CancellationToken())
		{
			await _channelAPI.CreateMessageAsync(gatewayEvent.ChannelID.Value, $"interaction received - button with ID: {gatewayEvent.Data.Value.CustomID.Value} was pressed", ct:ct);

			await _interactionAPI.CreateInteractionResponseAsync(gatewayEvent.ID, gatewayEvent.Token, new InteractionResponse(InteractionCallbackType.ChannelMessageWithSource, new InteractionApplicationCommandCallbackData
			{
				Content = "Acknowledged",
				Flags = MessageFlags.Ephemeral
			}), ct);
			return Result.FromSuccess();
		}
	}
}