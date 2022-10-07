namespace HomeAutomationApi.Services;

public interface IPhilipsHueService
{
	IAsyncEnumerable<string> GetAliasesAsync(CancellationToken? cancellationToken = null);
	Task<bool> GetPowerAsync(string alias, CancellationToken? cancellationToken = null);
	Task SetPowerAsync(string alias, bool on, CancellationToken? cancellationToken = null);
}
