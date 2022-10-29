using Dawn;
using Microsoft.AspNetCore.Mvc;

namespace HomeAutomationApi.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public class GlobalCacheController : ControllerBase
{
	private readonly Helpers.GlobalCache.IService _service;

	public GlobalCacheController(Helpers.GlobalCache.IService service)
	{
		_service = Guard.Argument(service).NotNull().Value;
	}

	[HttpPut("{alias:minlength(1)}")]
	public async Task<IActionResult> Put(string alias)
	{
		try { await _service.SendMessageAsync(alias); }
		catch (KeyNotFoundException)
		{
			return BadRequest(new { message = "unknown alias", alias, });
		}
		return Ok();
	}
}
