namespace HomeAutomationApi.Services;

public interface IPhilipsHueService
{
	IAsyncEnumerable<string> GetAliasesAsync(CancellationToken cancellationToken = default);
	Task<bool> GetPowerAsync(string alias, CancellationToken cancellationToken = default);
	Task SetPowerAsync(string alias, bool on, CancellationToken cancellationToken = default);
}
