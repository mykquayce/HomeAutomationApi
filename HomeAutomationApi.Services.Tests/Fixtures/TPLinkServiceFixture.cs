namespace HomeAutomationApi.Services.Tests.Fixtures;

public sealed class TPLinkServiceFixture : IDisposable
{
	private readonly ServiceProviderFixture _serviceProviderFixture = new();
	public ITPLinkService Service => _serviceProviderFixture.GetRequiredService<ITPLinkService>();
	public void Dispose() => _serviceProviderFixture.Dispose();
}
