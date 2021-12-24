using Dawn;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.NetworkInformation;

namespace HomeAutomationApi.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ElgatoController : ControllerBase
{
	private readonly ILogger<ElgatoController> _logger;
	private readonly IReadOnlyDictionary<string, PhysicalAddress> _aliasesLookup;
	private readonly Helpers.NetworkDiscoveryApi.IClient _networkDiscoveryApiClient;
	private readonly Helpers.Elgato.IElgatoClient _elgatoClient;

	public ElgatoController(
		ILogger<ElgatoController> logger,
		IOptions<Models.AliasesLookup> aliasesLookupOptions,
		Helpers.NetworkDiscoveryApi.IClient networkDiscoveryApiClient,
		Helpers.Elgato.IElgatoClient elgatoClient)
	{
		_logger = logger;
		_aliasesLookup = Guard.Argument(aliasesLookupOptions).NotNull().Wrap(o => o.Value)
			.NotNull().NotEmpty().DoesNotContainNull().Value;
		_networkDiscoveryApiClient = Guard.Argument(networkDiscoveryApiClient).NotNull().Value;
		_elgatoClient = Guard.Argument(elgatoClient).NotNull().Value;
	}

	[HttpGet]
	[Route("{alias:minlength(1)}")]
	public async Task<IActionResult> GetLightInfoAsync(string alias)
	{
		_logger.LogInformation("{route} : {arg}", nameof(GetLightInfoAsync), alias);

		using var cts = new CancellationTokenSource(millisecondsDelay: 3_000);

		var ip = await GetIPAddressAsync(alias, cts.Token);

		if (ip is null) return NotFound(new { alias, });

		var status = await _elgatoClient.GetLightAsync(ip, cts.Token);
		return Ok(new { on = status.on == 1, status.brightness, status.temperature, });
	}

	[HttpPut]
	[Route("{alias:minlength(1)}/power/{power:alpha:minlength(1)}")]
	public async Task<IActionResult> SetLightPowerAsync(string alias, PowerStatuses power)
	{
		_logger.LogInformation("{route} : {arg} {arg}", nameof(SetLightPowerAsync), alias, power);

		using var cts = new CancellationTokenSource(millisecondsDelay: 3_000);

		var ip = await GetIPAddressAsync(alias, cts.Token);

		if (ip is null) return NotFound(new { alias, });

		var status = await _elgatoClient.GetLightAsync(ip, cts.Token);

		status = power switch
		{
			PowerStatuses.Off => status with { on = 0, },
			PowerStatuses.On => status with { on = 1, },
			PowerStatuses.Toggle => status with { on = status.on == 1 ? (byte)0 : (byte)1, },
			_ => throw new NotSupportedException(),
		};

		await _elgatoClient.SetLightAsync(ip, status, cts.Token);

		status = await _elgatoClient.GetLightAsync(ip, cts.Token);

		return Ok(new { on = status.on == 1, status.brightness, status.temperature, });
	}

	private async Task<IPAddress?> GetIPAddressAsync(string alias, CancellationToken? cancellationToken = default)
	{
		var physicalAddress = _aliasesLookup.TryGetValue(alias, out var value) ? value : default;

		if (physicalAddress is null)
		{
			return default;
		}

		var response = await _networkDiscoveryApiClient.GetLeasesAsync(cancellationToken)
			.SingleAsync(r => r.physicalAddress.Equals(physicalAddress), cancellationToken ?? CancellationToken.None);

		return response?.ipAddress;
	}

	[Flags]
	public enum PowerStatuses : byte
	{
		None = 0,
		Off = 1,
		On = 2,
		Toggle = 4,
	}
}
