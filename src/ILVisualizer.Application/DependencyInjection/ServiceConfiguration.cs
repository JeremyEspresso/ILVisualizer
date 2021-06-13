using Microsoft.Extensions.DependencyInjection;

namespace ILVisualizer.Application.DependencyInjection
{
	public static class ServiceConfiguration
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			// All Application layer services will be registered here. Remove this comment if one is added
			return services;
		}
	}
}