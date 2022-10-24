namespace HomeAutomationApi.Services.Tests.Fixtures;

public sealed class HttpClientFixture : IDisposable
{
	public HttpClientFixture()
	{
		var handler = new HttpClientHandler { AllowAutoRedirect = false, };
		HttpClient = new HttpClient(handler);
	}

	public HttpClient HttpClient { get; }

	public void Dispose() => HttpClient.Dispose();
}
