using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeAutomationApi.Services.Tests.Fixtures;

public sealed class NetworkDiscoveryFixture : IDisposable
{
	private readonly IServiceProvider _serviceProvider;

	public NetworkDiscoveryFixture()
	{
		var configuration = new ConfigurationBuilder()
			.AddUserSecrets<NetworkDiscoveryFixture>()
			.Build();

		_serviceProvider = new ServiceCollection()
			.AddNetworkDiscoveryApi(configuration)
			.BuildServiceProvider();

		Client = _serviceProvider.GetRequiredService<Helpers.NetworkDiscoveryApi.IClient>();
		Service = _serviceProvider.GetRequiredService<Helpers.NetworkDiscoveryApi.IService>();
	}

	public Helpers.NetworkDiscoveryApi.IClient Client { get; }
	public Helpers.NetworkDiscoveryApi.IService Service { get; }

	public void Dispose() => (_serviceProvider as IDisposable)?.Dispose();
}
