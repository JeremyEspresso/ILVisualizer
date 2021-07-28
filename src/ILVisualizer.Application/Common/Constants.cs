namespace ILVisualizer.Application.Common
{
	public static class ApplicationConstants
	{
		// Discord
		public const string DiscordTokenEnvironmentVariable = "DISCORD_TOKEN";
		public const string DiscordPrefixEnvironmentVariable = "DISCORD_PREFIX";

		// Environment
		public const string EnvironmentNameEnvironmentVariable = "ENV";
		public const string ApplicationNameEnvironmentVariable = "APP_NAME";
	}

	public static class CommandConstants
	{
		// Command Context Key
		public const string Ctx = nameof(Ctx);
	}
}