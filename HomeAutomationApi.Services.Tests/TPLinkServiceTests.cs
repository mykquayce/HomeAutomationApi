namespace HomeAutomationApi.Services.Tests;

[Collection(nameof(NonParallelCollectionDefinitionClass))]
public class TPLinkServiceTests
	: IClassFixture<Fixtures.NetworkDiscoveryFixture>,
	IClassFixture<Fixtures.TPLinkFixture>
{
	private readonly Helpers.NetworkDiscoveryApi.IService _networkDiscoveryApiService;
	private readonly Helpers.TPLink.ITPLinkService _tpLinkService;

	public TPLinkServiceTests(
		Fixtures.NetworkDiscoveryFixture networkDiscoveryFixture,
		Fixtures.TPLinkFixture tpLinkFixture)
	{
		_networkDiscoveryApiService = networkDiscoveryFixture.Service;
		_tpLinkService = tpLinkFixture.Service;
	}

	[Theory]
	[InlineData("amp")]
	[InlineData("vr front")]
	[InlineData("vr rear")]
	public async Task GetStateTests(string alias)
	{
		(_, _, var ip, _, _) = await _networkDiscoveryApiService.GetLeaseAsync(alias);
		await _tpLinkService.GetStateAsync(ip);
	}
}
