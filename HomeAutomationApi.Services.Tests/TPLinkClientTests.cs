using System.Net;

namespace HomeAutomationApi.Services.Tests;

[Collection(nameof(NonParallelCollectionDefinitionClass))]
public class TPLinkClientTests : IClassFixture<Fixtures.TPLinkFixture>
{
	private readonly Helpers.TPLink.ITPLinkClient _tpLinkClient;

	public TPLinkClientTests(Fixtures.TPLinkFixture tpLinkFixture)
	{
		_tpLinkClient = tpLinkFixture.Client;
	}

	[Theory]
	[InlineData(new byte[4] { 192, 168, 1, 143, })]
	public async Task GetSystemInfoTests(byte[] address)
	{
		var ip = new IPAddress(address);
		var info = await _tpLinkClient.GetSystemInfoAsync(ip);

		Assert.NotNull(info);
		Assert.NotNull(info.Alias);
	}
}
