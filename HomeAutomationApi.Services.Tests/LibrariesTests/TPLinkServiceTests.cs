using System.Net;
using System.Net.NetworkInformation;

namespace HomeAutomationApi.Services.Tests.LibrariesTests;

[Collection(nameof(NonParallelCollectionDefinitionClass))]
public class TPLinkServiceTests
	: IClassFixture<Fixtures.TPLinkFixture>
{
	private readonly Helpers.TPLink.IService _sut;

	public TPLinkServiceTests(Fixtures.TPLinkFixture fixture)
	{
		_sut = fixture.Service;
	}

	[Fact]
	public async Task DiscoverTests()
	{
		var devices = await _sut.DiscoverAsync().ToArrayAsync();

		Assert.NotEmpty(devices);

		foreach ((string host, IPAddress ip, PhysicalAddress mac) in devices)
		{
			Assert.NotNull(host);
			Assert.NotEmpty(host);
			Assert.NotEqual(default, ip);
			Assert.NotEqual(IPAddress.None, ip);
			Assert.NotEqual(default, mac);
			Assert.NotEqual(PhysicalAddress.None, mac);
		}
	}

	private IAsyncEnumerable<IPAddress> GetDevicesIPAddressesAsync()
		=> _sut.DiscoverAsync().Select(tuple => tuple.Item2);

	[Fact]
	public async Task GetRealtimeDataTests()
	{
		var ips = GetDevicesIPAddressesAsync();

		await foreach (var ip in ips)
		{
			var tuples = await _sut.GetRealtimeDataAsync(ip).ToArrayAsync();
			Assert.Single(tuples);

			var (amps, volts, watts) = tuples[0];

			Assert.InRange(volts, 230, 255);
			if (amps > 0)
			{
				Assert.InRange(amps, .001, 1_000);
				Assert.InRange(watts, .1, 10);
			}
			else
			{
				Assert.Equal(0, amps);
				Assert.Equal(0, watts);
			}
		}
	}

	[Fact]
	public async Task GetStateTests()
	{
		var ips = GetDevicesIPAddressesAsync();

		await foreach (var ip in ips)
		{
			var states = await _sut.GetStateAsync(ip).ToArrayAsync();
			Assert.Single(states);
		}
	}
}
