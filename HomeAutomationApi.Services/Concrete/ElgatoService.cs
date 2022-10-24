using Dawn;
using System.Net;

namespace HomeAutomationApi.Services.Concrete;

public class ElgatoService : IElgatoService
{
	private readonly Helpers.Elgato.IService _service;
	private readonly Helpers.NetworkDiscovery.IClient _networkDiscoveryClient;

	public ElgatoService(Helpers.Elgato.IService service, Helpers.NetworkDiscovery.IClient networkDiscoveryClient)
	{
		_service = Guard.Argument(service).NotNull().Value;
		_networkDiscoveryClient = Guard.Argument(networkDiscoveryClient).NotNull().Value;
	}

	public async Task ToggleStateAsync(string alias, CancellationToken cancellationToken = default)
	{
		Guard.Argument(alias).NotNull().NotEmpty().NotWhiteSpace();
		(_, _, IPAddress ip, _, _) = await _networkDiscoveryClient.ResolveAsync(alias, cancellationToken);
		await _service.TogglePowerStateAsync(ip, cancellationToken);
	}

	public async Task SetStateAsync(string alias, bool state, CancellationToken cancellationToken = default)
	{
		Guard.Argument(alias).NotNull().NotEmpty().NotWhiteSpace();
		(_, _, IPAddress ip, _, _) = await _networkDiscoveryClient.ResolveAsync(alias, cancellationToken);
		await _service.SetPowerStateAsync(ip, state, cancellationToken);
	}
}
