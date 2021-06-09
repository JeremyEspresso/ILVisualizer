using ILVisualizer.Application.Common.Interfaces;
using ILVisualizer.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ILVisualizer.Application.DependencyInjection
{
	public static class ServiceConfiguration
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddSingleton<ICommandHandlerService, CommandHandlerService>();
			return services;
		}
	}
}