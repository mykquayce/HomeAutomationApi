namespace HomeAutomationApi.Services.Tests.Fixtures;

public sealed class TPLinkServiceFixture : IDisposable
{
	private readonly NetworkDiscoveryFixture _networkDiscoveryFixture;
	private readonly TPLinkFixture _tpLinkFixture;

	public TPLinkServiceFixture()
	{
		_networkDiscoveryFixture = new();
		_tpLinkFixture = new();

		Service = new Concrete.TPLinkService(_networkDiscoveryFixture.Service, _tpLinkFixture.Service);
	}

	public ITPLinkService Service { get; }

	public void Dispose()
	{
		_networkDiscoveryFixture.Dispose();
		_tpLinkFixture.Dispose();
	}
}
