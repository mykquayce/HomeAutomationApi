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

		PhysicalAddress mac;
		{
			if (!_aliasesLookup.TryGetValue(alias, out mac!))
			{
				return NotFound(new { alias, });
			}
		}

		IPAddress ip;
		{
			var response = await _networkDiscoveryApiClient.GetLeasesAsync(cts.Token)
				.SingleOrDefaultAsync(response => response.physicalAddress.Equals(mac));

			if (response is null) return NotFound(new { mac, });
			(_, _, ip, _, _) = response;
		}

		var status = await _elgatoClient.GetLightAsync(ip, cts.Token);
		return Ok(new { on = status.on == 1, status.brightness, status.temperature, });
	}
}
