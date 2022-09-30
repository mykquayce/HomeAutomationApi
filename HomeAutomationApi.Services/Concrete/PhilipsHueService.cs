using Dawn;

namespace HomeAutomationApi.Services.Concrete;

public class PhilipsHueService : IPhilipsHueService
{
	private readonly Helpers.PhilipsHue.IService _service;

	public PhilipsHueService(Helpers.PhilipsHue.IService service)
	{
		_service = Guard.Argument(service).NotNull().Value;
	}

	public Task<bool> GetPowerAsync(string alias, CancellationToken? cancellationToken = null)
	{
		return _service.GetLightPowerAsync(alias, cancellationToken);
	}

	public Task SetPowerAsync(string alias, bool on, CancellationToken? cancellationToken = null)
	{
		return _service.SetLightPowerAsync(alias, on: on, cancellationToken);
	}
}
