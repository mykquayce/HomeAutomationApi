namespace HomeAutomationApi.Services;

public interface IElgatoService
{
	Task ToggleStateAsync(string alias, CancellationToken cancellationToken = default);
	Task SetStateAsync(string alias, bool state, CancellationToken cancellationToken = default);
}
