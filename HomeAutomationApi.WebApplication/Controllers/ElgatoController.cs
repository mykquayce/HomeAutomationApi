using Dawn;
using Microsoft.AspNetCore.Mvc;

namespace HomeAutomationApi.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public class ElgatoController : ControllerBase
{
	private readonly Services.IElgatoService _service;

	public ElgatoController(Services.IElgatoService service)
	{
		_service = Guard.Argument(service).NotNull().Value;
	}

	[HttpPut("{alias}/{state}")]
	public IActionResult Put(string alias, States state)
	{
		_ = state switch
		{
			States.Off => _service.SetStateAsync(alias, state: false),
			States.On => _service.SetStateAsync(alias, state: true),
			States.Toggle => _service.ToggleStateAsync(alias),
			_ => throw new ArgumentOutOfRangeException(nameof(state), state, "allowed states: on, off, toggle"),
		};

		return Ok();
	}
}
