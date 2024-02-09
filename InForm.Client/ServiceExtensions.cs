using InForm.Client.Features.Forms.Contracts;
using InForm.Client.Features.Forms.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace InForm.Client
{
	public static class ServiceExtensions
	{
		public static IServiceCollection AddInFormServer(this IServiceCollection services)
		{
			services.AddInFormServer((_, _) => { });
			return services;
		}

		public static IServiceCollection AddInFormServer(this IServiceCollection services,
													     Action<IServiceProvider, HttpClient> config)
		{
			services.AddHttpClient<IFormsService, FormsService>(config);
			return services;
		}
	}
}
