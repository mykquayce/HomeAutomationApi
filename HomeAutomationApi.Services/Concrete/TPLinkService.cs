using Dawn;

namespace HomeAutomationApi.Services.Concrete;

public class TPLinkService : ITPLinkService
{
	private readonly Helpers.NetworkDiscoveryApi.IService _networkDiscoveryService;
	private readonly Helpers.TPLink.ITPLinkService _tpLinkService;

	public TPLinkService(
		Helpers.NetworkDiscoveryApi.IService networkDiscoveryService,
		Helpers.TPLink.ITPLinkService tpLinkService)
	{
		_networkDiscoveryService = Guard.Argument(networkDiscoveryService).NotNull().Value;
		_tpLinkService = Guard.Argument(tpLinkService).NotNull().Value;
	}

	public async Task SetStateAsync(string alias, bool state, CancellationToken? cancellationToken = null)
	{
		cancellationToken ??= CancellationToken.None;
		(_, _, var ip, _, _) = await _networkDiscoveryService.GetLeaseAsync(alias, cancellationToken);
		await _tpLinkService.SetStateAsync(ip, state);
	}
}
