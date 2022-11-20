using System.Net;
using System.Net.NetworkInformation;

namespace HomeAutomationApi.Services.Tests.LibrariesTests;

[Collection(nameof(NonParallelCollectionDefinitionClass))]
public class TPLinkClientTests : IClassFixture<Fixtures.TPLinkFixture>
{
	private readonly Helpers.TPLink.IClient _sut;
	private readonly Helpers.TPLink.IDiscoveryClient _discoveryClient;

	public TPLinkClientTests(Fixtures.TPLinkFixture fixture)
	{
		_sut = fixture.Client;
		_discoveryClient = fixture.DiscoveryClient;
	}

	private IAsyncEnumerable<IPAddress> GetDevicesIPAddressesAsync()
		=> _discoveryClient.DiscoverAsync().Select(tuple => tuple.Item2);

	[Fact]
	public async Task GetSystemInfoTests()
	{
		var ips = GetDevicesIPAddressesAsync();

		await foreach (var ip in ips)
		{
			var info = await _sut.GetSystemInfoAsync(ip).ToArrayAsync();

			Assert.Single(info);
			(string alias, PhysicalAddress mac, string model, int relay_state) = info[0];

			Assert.NotNull(alias);
			Assert.NotEmpty(alias);
			Assert.NotNull(mac);
			Assert.NotEqual(default, mac);
			Assert.NotEqual(PhysicalAddress.None, mac);
			Assert.NotNull(model);
			Assert.NotEmpty(model);
			Assert.InRange(relay_state, 0, 1);
		}
	}
}
