using Microsoft.Extensions.DependencyInjection;

namespace HomeAutomationApi.Services.Tests.Fixtures;

public sealed class TPLinkFixture : IDisposable
{
	private readonly ServiceProviderFixture _serviceProviderFixture = new();

	public Helpers.TPLink.ITPLinkClient Client => _serviceProviderFixture.GetRequiredService<Helpers.TPLink.ITPLinkClient>();
	public Helpers.TPLink.ITPLinkService Service => _serviceProviderFixture.GetRequiredService<Helpers.TPLink.ITPLinkService>();
	public void Dispose() => _serviceProviderFixture.Dispose();
}
