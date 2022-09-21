using Microsoft.Extensions.DependencyInjection;

namespace HomeAutomationApi.Services.Tests.Fixtures;

public sealed class TPLinkFixture : IDisposable
{
	private readonly IServiceProvider _serviceProvider;

	public TPLinkFixture()
	{
		_serviceProvider = new ServiceCollection()
			.AddTPLink(Helpers.TPLink.Config.Defaults)
			.BuildServiceProvider();

		Client = _serviceProvider.GetRequiredService<Helpers.TPLink.ITPLinkClient>();
		Service = _serviceProvider.GetRequiredService<Helpers.TPLink.ITPLinkService>();
	}

	public Helpers.TPLink.ITPLinkClient Client { get; }
	public Helpers.TPLink.ITPLinkService Service { get; }

	public void Dispose() => (_serviceProvider as IDisposable)?.Dispose();
}
