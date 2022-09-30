namespace HomeAutomationApi.Services;

public interface IPhilipsHueService
{
	Task<bool> GetPowerAsync(string alias, CancellationToken? cancellationToken = null);
	Task SetPowerAsync(string alias, bool on, CancellationToken? cancellationToken = null);
}
