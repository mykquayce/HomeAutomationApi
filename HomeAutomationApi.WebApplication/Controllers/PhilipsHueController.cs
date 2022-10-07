using Microsoft.AspNetCore.Mvc;

namespace HomeAutomationApi.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public class PhilipsHueController : ControllerBase
{
	private readonly Services.IPhilipsHueService _service;

	public PhilipsHueController(Services.IPhilipsHueService service)
	{
		_service = service;
	}

	[HttpGet("aliases")]
	public async Task<IActionResult> Aliases()
	{
		var aliases = await _service.GetAliasesAsync().ToListAsync();

		if (aliases.Any())
		{
			return Ok(aliases);
		}

		return NotFound();
	}

	[HttpPut("{alias}/off")]
	public async Task<IActionResult> Off(string alias)
	{
		await _service.SetPowerAsync(alias, on: false);
		return Ok();
	}

	[HttpPut("{alias}/on")]
	public async Task<IActionResult> On(string alias)
	{
		await _service.SetPowerAsync(alias, on: true);
		return Ok();
	}
}
