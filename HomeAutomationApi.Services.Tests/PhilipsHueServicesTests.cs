namespace HomeAutomationApi.Services.Tests;

public class PhilipsHueServicesTests : IClassFixture<Fixtures.PhilipsHueServiceFixture>
{
	private readonly IPhilipsHueService _sut;

	public PhilipsHueServicesTests(Fixtures.PhilipsHueServiceFixture fixture)
	{
		_sut = fixture.Service;
	}

	[Theory]
	[InlineData("strip")]
	[InlineData("wall right")]
	public async Task OffTests(string alias)
	{
		using var cts = new CancellationTokenSource(millisecondsDelay: 500);
		await _sut.SetPowerAsync(alias, on: false, cts.Token);
		var on = await _sut.GetPowerAsync(alias, cts.Token);
		Assert.False(on);
	}

	[Theory]
	[InlineData("strip")]
	public async Task OnTests(string alias)
	{
		using var cts = new CancellationTokenSource(millisecondsDelay: 500);
		await _sut.SetPowerAsync(alias, on: true, cts.Token);
		var on = await _sut.GetPowerAsync(alias, cts.Token);
		Assert.True(on);
	}
}
