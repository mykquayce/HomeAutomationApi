using System.Net;

namespace HomeAutomationApi.Services.Tests.LibrariesTests;

[Collection(nameof(NonParallelCollectionDefinitionClass))]
public class NetworkDiscoveryClientTests : IClassFixture<Fixtures.NetworkDiscoveryFixture>
{
	private readonly Helpers.NetworkDiscovery.IClient _sut;

	public NetworkDiscoveryClientTests(Fixtures.NetworkDiscoveryFixture fixture)
	{
		_sut = fixture.Client;
	}

	[Theory]
	[InlineData("vr front")]
	public async Task GetLeasesTests(string alias)
	{
		Helpers.Networking.Models.DhcpLease dhcp;
		{
			using var cts = new CancellationTokenSource(millisecondsDelay: 10_000);
			dhcp = await _sut.ResolveAsync(alias, cts.Token);
		}

		Assert.NotEqual(IPAddress.None, dhcp.IPAddress);
	}
}
