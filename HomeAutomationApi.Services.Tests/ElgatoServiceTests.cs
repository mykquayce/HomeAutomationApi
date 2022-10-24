namespace HomeAutomationApi.Services.Tests;

public sealed class ElgatoServiceTests
	: IClassFixture<Fixtures.HttpClientFixture>,
	IClassFixture<Fixtures.NetworkDiscoveryFixture>
{
	private readonly IElgatoService _sut;

	public ElgatoServiceTests(
		Fixtures.HttpClientFixture httpClientFixture,
		Fixtures.NetworkDiscoveryFixture networkDiscoveryFixture)
	{
		var config = Helpers.Elgato.Config.Defaults;
		var client = new Helpers.Elgato.Concrete.Client(config, httpClientFixture.HttpClient);
		var service = new Helpers.Elgato.Concrete.Service(client);

		_sut = new Concrete.ElgatoService(
			service,
			networkDiscoveryFixture.Client);
	}

	[Theory]
	[InlineData("keylight")]
	[InlineData("lightstrip")]
	public Task ToggleTests(string alias) => _sut.ToggleStateAsync(alias);
}
