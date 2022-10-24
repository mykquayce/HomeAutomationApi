namespace HomeAutomationApi.Services.Tests.Fixtures;

public sealed class PhilipsHueServiceFixture : IDisposable
{
	private readonly ServiceProviderFixture _serviceProviderFixture = new();
	public IPhilipsHueService Service => _serviceProviderFixture.GetRequiredService<IPhilipsHueService>();
	public void Dispose() => _serviceProviderFixture.Dispose();
}
