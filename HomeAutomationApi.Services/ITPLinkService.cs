namespace HomeAutomationApi.Services;

public interface ITPLinkService
{
	Task SetStateAsync(string alias, bool state, CancellationToken? cancellationToken = null);
}
