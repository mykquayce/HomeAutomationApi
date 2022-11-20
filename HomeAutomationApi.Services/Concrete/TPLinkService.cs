using Dawn;

namespace HomeAutomationApi.Services.Concrete;

public class TPLinkService : ITPLinkService
{
	private readonly Helpers.NetworkDiscovery.IClient _networkDiscoveryClient;
	private readonly Helpers.TPLink.IService _tpLinkService;

	public TPLinkService(
		Helpers.NetworkDiscovery.IClient networkDiscoveryClient,
		Helpers.TPLink.IService tpLinkService)
	{
		_networkDiscoveryClient = Guard.Argument(networkDiscoveryClient).NotNull().Value;
		_tpLinkService = Guard.Argument(tpLinkService).NotNull().Value;
	}

	public async Task SetStateAsync(string alias, bool state, CancellationToken cancellationToken = default)
	{
		(_, _, var ip, _, _) = await _networkDiscoveryClient.ResolveAsync(alias, cancellationToken);
		await _tpLinkService.SetStateAsync(ip, state, cancellationToken);
	}
}
