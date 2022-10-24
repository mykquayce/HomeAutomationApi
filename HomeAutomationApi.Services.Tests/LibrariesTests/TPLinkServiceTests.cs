namespace HomeAutomationApi.Services.Tests.LibrariesTests;

[Collection(nameof(NonParallelCollectionDefinitionClass))]
public class TPLinkServiceTests
	: IClassFixture<Fixtures.TPLinkFixture>
{
	private readonly Helpers.TPLink.ITPLinkService _tpLinkService;

	public TPLinkServiceTests(
		Fixtures.TPLinkFixture tpLinkFixture)
	{
		_tpLinkService = tpLinkFixture.Service;
	}

	[Theory]
	[InlineData("amp")]
	[InlineData("vr front")]
	[InlineData("vr rear")]
	public Task GetStateTests(string alias)
		=> _tpLinkService.GetStateAsync(alias);
}
