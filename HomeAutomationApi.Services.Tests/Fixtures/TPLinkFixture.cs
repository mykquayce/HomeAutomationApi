namespace HomeAutomationApi.Services.Tests.Fixtures;

public sealed class TPLinkFixture : IDisposable
{
	private readonly ServiceProviderFixture _serviceProviderFixture;

	public TPLinkFixture()
	{
		_serviceProviderFixture = new();

		Client = get<Helpers.TPLink.IClient>();
		DiscoveryClient = get<Helpers.TPLink.IDiscoveryClient>();
		Service = get<Helpers.TPLink.IService>();

		T get<T>() where T : notnull
			=> _serviceProviderFixture.GetRequiredService<T>();
	}

	public Helpers.TPLink.IClient Client { get; }
	public Helpers.TPLink.IDiscoveryClient DiscoveryClient { get; }
	public Helpers.TPLink.IService Service { get; }

	public void Dispose() => _serviceProviderFixture.Dispose();
}
