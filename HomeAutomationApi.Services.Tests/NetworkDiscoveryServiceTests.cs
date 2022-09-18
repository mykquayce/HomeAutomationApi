using System.Net;

namespace HomeAutomationApi.Services.Tests;

public class NetworkDiscoveryServiceTests : IClassFixture<Fixtures.NetworkDiscoveryFixture>
{
	private readonly Helpers.NetworkDiscoveryApi.IService _sut;

	public NetworkDiscoveryServiceTests(Fixtures.NetworkDiscoveryFixture fixture)
	{
		_sut = fixture.Service;
	}

	[Theory]
	[InlineData("vr front")]
	public async Task GetLeasesTests(string alias)
	{
		Helpers.NetworkDiscoveryApi.Models.DhcpResponseObject dhcp;
		{
			using var cts = new CancellationTokenSource(millisecondsDelay: 10_000);
			dhcp = await _sut.GetLeaseAsync(alias, cts.Token);
		}

		Assert.NotEqual(IPAddress.None, dhcp.ipAddress);
	}
}
