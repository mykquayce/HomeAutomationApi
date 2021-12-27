using System.Text.Json;
using Xunit;

namespace HomeAutomationApi.WebApi.Tests;

public class IntegrationTests : IClassFixture<Fixtures.IntegrationTestsFixture>
{
	private readonly HttpClient _httpClient;

	public IntegrationTests(Fixtures.IntegrationTestsFixture fixture)
	{
		_httpClient = fixture.HttpClient;
	}

	[Theory]
	[InlineData("brightest", 1)]
	[InlineData("brighter", .83)]
	[InlineData("bright", .67)]
	[InlineData("half", .5)]
	[InlineData("dim", .34)]
	[InlineData("dimmer", .17)]
	[InlineData("dimmest", 0)]
	public async Task BrightnessTests(string brightness, double expected)
	{
		Light? light;
		{
			// Arrange
			var relativeUriString = "/elgato/elgato/brightness/" + brightness;

			// Act
			var response = await _httpClient.PutAsync(relativeUriString, default);

			// Assert
			await using var stream = await response.Content.ReadAsStreamAsync();
			stream.Position = 0;
			light = await JsonSerializer.DeserializeAsync<Light>(stream);
		}

		Assert.NotNull(light);
		Assert.Equal(expected, light!.brightness, precision: 2);
	}

	[Theory]
	[InlineData("off", false)]
	[InlineData("on", true)]
	public async Task PowerTests(string power, bool expected)
	{
		Light? light;
		{
			// Arrange
			var relativeUriString = "/elgato/elgato/power/" + power;

			// Act
			var response = await _httpClient.PutAsync(relativeUriString, default);

			// Assert
			await using var stream = await response.Content.ReadAsStreamAsync();
			stream.Position = 0;
			light = await JsonSerializer.DeserializeAsync<Light>(stream);
		}

		Assert.NotNull(light);
		Assert.Equal(expected, light!.on);
	}

	[Theory]
	[InlineData("coolest", 7_000)]
	[InlineData("cooler", 6_317)]
	[InlineData("cool", 5_633)]
	[InlineData("half", 4_950)]
	[InlineData("warm", 4_267)]
	[InlineData("warmer", 3_583)]
	[InlineData("warmest", 2_900)]
	public async Task TemperaturesTests(string temperature, int expected)
	{
		Light? light;
		{
			// Arrange
			var relativeUriString = "/elgato/elgato/temperature/" + temperature;

			// Act
			var response = await _httpClient.PutAsync(relativeUriString, default);

			// Assert
			await using var stream = await response.Content.ReadAsStreamAsync();
			stream.Position = 0;
			light = await JsonSerializer.DeserializeAsync<Light>(stream);
		}

		Assert.NotNull(light);
		Assert.InRange(light!.kelvins, expected - 10, expected + 10);
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "3rd-party")]
	public record Light(bool on, double brightness, short kelvins);
}
