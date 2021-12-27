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
	private readonly IReadOnlyDictionary<string, double> _brightnessesLookup;
	private readonly IReadOnlyDictionary<string, short> _temperaturesLookup;
	private readonly Helpers.NetworkDiscoveryApi.IClient _networkDiscoveryApiClient;
	private readonly Helpers.Elgato.IElgatoService _elgatoService;

	public ElgatoController(
		ILogger<ElgatoController> logger,
		IOptions<Models.AliasesLookup> aliasesLookupOptions,
		IOptions<Models.BrightnessesLookup> brightnessesLookupOptions,
		IOptions<Models.TemperaturesLookup> temperaturesLookupOptions,
		Helpers.NetworkDiscoveryApi.IClient networkDiscoveryApiClient,
		Helpers.Elgato.IElgatoService elgatoService)
	{
		_logger = logger;
		_aliasesLookup = Guard.Argument(aliasesLookupOptions).NotNull().Wrap(o => o.Value)
			.NotNull().NotEmpty().DoesNotContainNull().Value;
		_brightnessesLookup = Guard.Argument(brightnessesLookupOptions).NotNull().Wrap(o => o.Value)
			.NotNull().NotEmpty().DoesNotContainNull().Value;
		_temperaturesLookup = Guard.Argument(temperaturesLookupOptions).NotNull().Wrap(o => o.Value)
			.NotNull().NotEmpty().DoesNotContainNull().Value;
		_networkDiscoveryApiClient = Guard.Argument(networkDiscoveryApiClient).NotNull().Value;
		_elgatoService = Guard.Argument(elgatoService).NotNull().Value;
	}

	[HttpGet]
	[Route("{alias:minlength(1)}")]
	public async Task<IActionResult> GetLightInfoAsync(string alias)
	{
		_logger.LogInformation("{route} : {arg}", nameof(GetLightInfoAsync), alias);

		using var cts = new CancellationTokenSource(millisecondsDelay: 3_000);

		var ip = await GetIPAddressAsync(alias, cts.Token);

		if (ip is null) return NotFound(new { alias, });

		var (on, brightness, kelvins) = await _elgatoService.GetLightSettingsAsync(ip, cts.Token);
		return Ok(new { on, brightness, kelvins, });
	}

	[HttpPut]
	[Route("{alias:minlength(1)}/power/{power:alpha:minlength(1)}")]
	public async Task<IActionResult> SetLightPowerAsync(string alias, PowerStatuses power)
	{
		_logger.LogInformation("{route} : {arg} {arg}", nameof(SetLightPowerAsync), alias, power);

		using var cts = new CancellationTokenSource(millisecondsDelay: 3_000);

		var ip = await GetIPAddressAsync(alias, cts.Token);

		if (ip is null) return NotFound(new { alias, });

		var task = power switch
		{
			PowerStatuses.Off => _elgatoService.SetPowerStateAsync(ip, false, cts.Token),
			PowerStatuses.On => _elgatoService.SetPowerStateAsync(ip, true, cts.Token),
			PowerStatuses.Toggle => _elgatoService.TogglePowerStateAsync(ip, cts.Token),
			_ => throw new ArgumentOutOfRangeException(nameof(power), power, $"unknown {nameof(power)} value: {power}"),
		};

		await task;

		var (on, brightness, kelvins) = await _elgatoService.GetLightSettingsAsync(ip, cts.Token);

		return Ok(new { on, brightness, kelvins, });
	}

	[HttpPut]
	[Route("{alias:minlength(1)}/brightness/{brightness:minlength(1)}")]
	public async Task<IActionResult> SetBrightnessAsync(string alias, [FromRoute(Name = "brightness")] string brightnessString)
	{
		_logger.LogInformation("{route} : {arg} {arg}", nameof(SetBrightnessAsync), alias, brightnessString);

		if (!_brightnessesLookup.TryGetValue(brightnessString, out var brightness))
		{
			return BadRequest(new { message = $"unrecognized {nameof(brightness)}: {brightnessString}.  should be one of: " + string.Join(',', _brightnessesLookup.Keys), });
		}

		using var cts = new CancellationTokenSource(millisecondsDelay: 3_000);

		var ip = await GetIPAddressAsync(alias, cts.Token);

		if (ip is null) return NotFound(new { alias, });

		await _elgatoService.SetBrightnessAsync(ip, brightness, cts.Token);

		(var on, brightness, var kelvins) = await _elgatoService.GetLightSettingsAsync(ip, cts.Token);

		return Ok(new { on, brightness, kelvins, });
	}

	[HttpPut]
	[Route("{alias:minlength(1)}/temperature/{temperature:minlength(1)}")]
	public async Task<IActionResult> SetTemperatureAsync(string alias, [FromRoute(Name = "temperature")] string temperatureString)
	{
		_logger.LogInformation("{route} : {arg} {arg}", nameof(SetTemperatureAsync), alias, temperatureString);

		if (!_temperaturesLookup.TryGetValue(temperatureString, out var kelvins))
		{
			return BadRequest(new { message = $"unrecognized {nameof(kelvins)}: {temperatureString}.  should be one of: " + string.Join(',', _temperaturesLookup.Keys), });
		}

		using var cts = new CancellationTokenSource(millisecondsDelay: 3_000);

		var ip = await GetIPAddressAsync(alias, cts.Token);

		if (ip is null) return NotFound(new { alias, });

		await _elgatoService.SetTemperatureAsync(ip, kelvins, cts.Token);

		(var on, var brightness, kelvins) = await _elgatoService.GetLightSettingsAsync(ip, cts.Token);

		return Ok(new { on, brightness, kelvins, });
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
