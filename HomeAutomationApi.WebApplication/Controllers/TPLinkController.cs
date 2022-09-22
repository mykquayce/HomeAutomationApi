using Dawn;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeAutomationApi.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public class TPLinkController : ControllerBase
{
	private readonly Services.ITPLinkService _service;

	public TPLinkController(Services.ITPLinkService service)
	{
		_service = Guard.Argument(service).NotNull().Value;
	}

	// PUT api/<PlugController>/5
	[HttpPut("{alias}/{state}")]
	public IActionResult Put(string alias, [FromRoute(Name = "state")] string stateString)
	{
		var ok = Enum.TryParse<States>(stateString, ignoreCase: true, out var state);

		if (!ok) return BadRequest(new { state = stateString, message = $"allowed states: on, off, toggle", });

		switch (state)
		{
			case States.Off:
				_service.SetStateAsync(alias, false);
				break;
			case States.On:
				_service.SetStateAsync(alias, true);
				break;
		}

		return Ok();
	}
}

[Flags]
public enum States : byte
{
	None = 0,
	Off = 1,
	On = 2,
	Toggle = 4,
}
