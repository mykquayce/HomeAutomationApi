using Helpers.TPLink;
using Microsoft.Extensions.DependencyInjection;

namespace HomeAutomationApi.Services.Tests.Fixtures;

public sealed class TPLinkFixture : IDisposable
{
	private readonly IServiceProvider _serviceProvider;

	public TPLinkFixture()
	{
		_serviceProvider = new ServiceCollection()
			.AddTPLink(Config.Defaults)
			.BuildServiceProvider();

		Client = _serviceProvider.GetRequiredService<ITPLinkClient>();
		Service = _serviceProvider.GetRequiredService<ITPLinkService>();
	}

	public ITPLinkClient Client { get; }
	public ITPLinkService Service { get; }

	public void Dispose() => (_serviceProvider as IDisposable)?.Dispose();
}
