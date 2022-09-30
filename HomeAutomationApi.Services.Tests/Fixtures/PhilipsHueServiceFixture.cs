using Microsoft.Extensions.DependencyInjection;

namespace HomeAutomationApi.Services.Tests.Fixtures;

public sealed class PhilipsHueServiceFixture : IDisposable
{
	private readonly IServiceProvider _serviceProvider;

	public PhilipsHueServiceFixture()
	{
		_serviceProvider = new ServiceCollection()
			.AddPhilipsHue(new Uri("http://192.168.1.156"), "i35sdUz4iZI0XPWxbIdQKdp76t4cH8LOwUCtFcFJ")
			.BuildServiceProvider();

		var library = _serviceProvider.GetRequiredService<Helpers.PhilipsHue.IService>();
		Service = new Concrete.PhilipsHueService(library);
	}

	public IPhilipsHueService Service { get; }

	public void Dispose() => (_serviceProvider as IDisposable)?.Dispose();
}
