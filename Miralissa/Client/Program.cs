using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Miralissa.Client.Services;
using MudBlazor;
using MudBlazor.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Miralissa.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			builder.Services.AddMudServices(mudOpts =>
			{
				mudOpts.SnackbarConfiguration.PreventDuplicates = false;
				mudOpts.SnackbarConfiguration.NewestOnTop = false;
				mudOpts.SnackbarConfiguration.ShowCloseIcon = true;
				mudOpts.SnackbarConfiguration.VisibleStateDuration = 8_000;
				mudOpts.SnackbarConfiguration.HideTransitionDuration = 1_000;
				mudOpts.SnackbarConfiguration.ShowTransitionDuration = 500;
				mudOpts.SnackbarConfiguration.SnackbarVariant = Variant.Outlined;
				mudOpts.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
			});

			builder.Services.AddOptions();
			builder.Services.AddAuthorizationCore();
			builder.Services.AddScoped<IdentityAuthenticationStateProvider>();
			builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<IdentityAuthenticationStateProvider>());
			builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
			builder.Services.AddScoped<IPreparedService, PreparedSubscriptService>();
			builder.Services.AddScoped<IOrderedService, OrderedSubscriptService>();
			builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

			await builder.Build().RunAsync();
		}
	}
}
