using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace HomeAutomationApi.Services.Tests.Fixtures;

public sealed class ServiceProviderFixture : IDisposable
{
	private readonly IServiceProvider _serviceProvider;

	public ServiceProviderFixture()
	{
		IConfiguration configuration = new ConfigurationBuilder()
			.AddUserSecrets<NetworkDiscoveryFixture>()
			.Build();

		_serviceProvider = new ServiceCollection()
			.AddNetworkDiscovery(configuration.GetSection("identity"), configuration.GetSection("networkdiscovery"))
			.AddPhilipsHue(configuration.GetSection("philipshue"), provider =>
			{
				var disco = provider.GetRequiredService<Helpers.NetworkDiscovery.IClient>();
				(_, _, IPAddress ip, _, _) = disco.ResolveAsync("philipshue").GetAwaiter().GetResult();
				var uri = new UriBuilder("http", ip.ToString()).Uri;
				return uri;
			})
			.AddTPLink()
			.AddTransient<IPhilipsHueService, Concrete.PhilipsHueService>()
			.AddTransient<ITPLinkService, Concrete.TPLinkService>()
			.BuildServiceProvider();
	}

	public T GetRequiredService<T>() where T : notnull
		=> _serviceProvider.GetRequiredService<T>();

	public void Dispose() => ((ServiceProvider)_serviceProvider).Dispose();
}
