using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ILVisualizer.Application.DependencyInjection
{
	public static class ServiceConfiguration
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{

			return services;
		}
	}
}