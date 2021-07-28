using System.Threading.Tasks;
using Finite.Commands;
using Finite.Commands.AttributedModel;
using ILVisualizer.Application.Common;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;

namespace ILVisualizer.Bot.Modules
{
	public class TestModule : Module
	{
		private readonly IDiscordRestChannelAPI _channelAPI;
		private readonly ILogger<TestModule> _logger;

		public TestModule(IDiscordRestChannelAPI channelApi, ILogger<TestModule> logger)
		{
			_channelAPI = channelApi;
			_logger = logger;
		}

		[Command("ping")]
		public async ValueTask<ICommandResult> PingPongCommand()
		{
			IMessageCreate ctx = (Context.Items[CommandConstants.Ctx] as IMessageCreate)!;
			
			await _channelAPI.CreateMessageAsync(ctx.ChannelID, "Hello from ping command");
			
			_logger.LogDebug("ping command executed");
			return await new ValueTask<ICommandResult>(new NoContentCommandResult());
		}

		[Command("test")]
		public async ValueTask<ICommandResult> TestCommand()
		{
			IMessageCreate ctx = (Context.Items[CommandConstants.Ctx] as IMessageCreate)!;

			await _channelAPI.CreateMessageAsync(ctx.ChannelID, "test message", 
				components: new[]
				{
					new ActionRowComponent(new []
					{
						new ButtonComponent(ButtonComponentStyle.Primary, "button", CustomID: "unique_identifier")
					})
				});

			return await new ValueTask<ICommandResult>(new NoContentCommandResult());
		}
	}
}