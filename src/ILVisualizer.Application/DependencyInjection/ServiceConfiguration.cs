using ILVisualizer.Application.Common.Interfaces;
using ILVisualizer.Application.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ILVisualizer.Application.DependencyInjection
{
	public static class ServiceConfiguration
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddScoped<IILParserService, ILParserService>();
			services.AddScoped<IILProcessorService, ILProcessorService>();
			return services;
		}
	}
}