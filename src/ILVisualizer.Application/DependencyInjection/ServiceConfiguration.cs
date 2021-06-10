using ILVisualizer.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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