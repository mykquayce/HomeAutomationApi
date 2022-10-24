namespace HomeAutomationApi.Services.Tests.Fixtures;

public sealed class NetworkDiscoveryFixture : IDisposable
{
	private readonly ServiceProviderFixture _serviceProviderFixture = new();
	public Helpers.NetworkDiscovery.IClient Client => _serviceProviderFixture.GetRequiredService<Helpers.NetworkDiscovery.IClient>();
	public void Dispose() => _serviceProviderFixture.Dispose();
}
