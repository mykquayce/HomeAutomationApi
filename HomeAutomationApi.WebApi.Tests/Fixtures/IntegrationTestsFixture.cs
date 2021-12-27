using Microsoft.AspNetCore.Mvc.Testing;

namespace HomeAutomationApi.WebApi.Tests.Fixtures;

public sealed class IntegrationTestsFixture : IAsyncDisposable
{
	private readonly WebApplicationFactory<Program> _factory;

	public IntegrationTestsFixture()
	{
		_factory = new();
		HttpClient = _factory.CreateClient();
	}

	public HttpClient HttpClient { get; }

	public ValueTask DisposeAsync() => _factory.DisposeAsync();
}
