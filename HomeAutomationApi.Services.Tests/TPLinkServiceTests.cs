namespace HomeAutomationApi.Services.Tests;

public class TPLinkServiceTests : IClassFixture<Fixtures.TPLinkServiceFixture>
{
	private readonly ITPLinkService _sut;

	public TPLinkServiceTests(Fixtures.TPLinkServiceFixture fixture)
	{
		_sut = fixture.Service;
	}

	[Theory]
	[InlineData("vr front", false)]
	[InlineData("vr front", true)]
	public Task Test1(string alias, bool state)
	{
		return _sut.SetStateAsync(alias, state);
	}
}
