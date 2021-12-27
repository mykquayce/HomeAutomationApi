using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.NetworkInformation;
using System.Text.Json;
using Xunit;

namespace HomeAutomationApi.WebApi.Tests;

public class ConfigTests
{
	[Theory]
	[InlineData(@"{""elgato"":""3c6a9d14d765""}", new[] { "elgato", "3c6a9d14d765", })]
	public void JsonDeserializationTests(string json, params string[][] expected)
	{
		var jsonSerializerOptions = new JsonSerializerOptions
		{
			Converters =
			{
				new JsonPhysicalAddressConverter(),
			},
		};

		var dictionary = JsonSerializer.Deserialize<IReadOnlyDictionary<string, PhysicalAddress>>(json, jsonSerializerOptions);

		Assert.NotNull(dictionary);
		Assert.NotEmpty(dictionary);

		foreach (var (expectedKey, expectedValue) in expected)
		{
			Assert.True(dictionary!.ContainsKey(expectedKey));
			var expectedPhysicalAddress = PhysicalAddress.Parse(expectedValue);
			Assert.Equal(expectedPhysicalAddress, dictionary[expectedKey]);
		}
	}

	[Theory]
	[InlineData(@"{""elgato"":""3c6a9d14d765""}", new[] { "elgato", "3c6a9d14d765", })]
	public async Task ConfigurationBuilderTests(string json, params string[][] expected)
	{
		IServiceProvider serviceProvider;
		{
			IConfiguration configuration;
			{
				var bytes = System.Text.Encoding.UTF8.GetBytes(json);
				await using Stream stream = new MemoryStream(bytes);

				configuration = new ConfigurationBuilder()
					.AddJsonStream(stream)
					.Build();
			}

			serviceProvider = new ServiceCollection()
				.JsonConfig<Aliases>(configuration)
				.BuildServiceProvider();
		}

		var options = serviceProvider.GetService<IOptions<Aliases>>();

		Assert.NotNull(options);

		var dictionary = options!.Value;

		Assert.NotNull(dictionary);
		Assert.NotEmpty(dictionary);

		foreach (var (expectedKey, expectedValue) in expected)
		{
			Assert.True(dictionary!.ContainsKey(expectedKey));
			var expectedPhysicalAddress = PhysicalAddress.Parse(expectedValue);
			Assert.Equal(expectedPhysicalAddress, dictionary[expectedKey]);
		}
	}

	[Theory]
	[InlineData(@"{
    ""Dimmest"": 0,
    ""Dimmer"": 0.17,
    ""Dim"": 0.34,
    ""Half"": 0.5,
    ""Bright"": 0.67,
    ""Brighter"": 0.83,
    ""Brightest"": 1
  }", new[] { "Dimmest", "Dimmer", "Dim", "Half", "Bright", "Brighter", "Brightest", }, new[] { 0, .17, .34, .5, .67, .83, 1, })]
	public void BrightnessesTests_JsonSerializer(string json, string[] expectedKeys, double[] expectedValues)
	{
		var dictionary = JsonSerializer.Deserialize<IReadOnlyDictionary<string, double>>(json);
		Assert.NotNull(dictionary);
		Assert.NotEmpty(dictionary);
		Assert.All(dictionary!.Keys, key => Assert.Contains(key, expectedKeys));
		Assert.All(dictionary!.Values, value => Assert.Contains(value, expectedValues));
	}

	[Theory]
	[InlineData(@"{
    ""Dimmest"": 0,
    ""Dimmer"": 0.17,
    ""Dim"": 0.34,
    ""Half"": 0.5,
    ""Bright"": 0.67,
    ""Brighter"": 0.83,
    ""Brightest"": 1
  }", new[] { "Dimmest", "Dimmer", "Dim", "Half", "Bright", "Brighter", "Brightest", }, new[] { 0, .17, .34, .5, .67, .83, 1, })]
	public void BrightnessesTests_DataBinding(string json, string[] expectedKeys, double[] expectedValues)
	{
		IServiceProvider serviceProvider;
		{
			IConfiguration configuration;
			{
				var bytes = System.Text.Encoding.UTF8.GetBytes(json);
				using var stream = new MemoryStream(bytes);
				configuration = new ConfigurationBuilder()
					.AddJsonStream(stream)
					.Build();
			}

			serviceProvider = new ServiceCollection()
				.Configure<Brightnesses>(configuration)
				.BuildServiceProvider();
		}

		var options = serviceProvider.GetService<IOptions<Brightnesses>>();

		Assert.NotNull(options);

		var dictionary = options!.Value;

		Assert.NotNull(dictionary);
		Assert.NotEmpty(dictionary);
		Assert.All(dictionary!.Keys, key => Assert.Contains(key, expectedKeys));
		Assert.All(dictionary!.Values, value => Assert.Contains(value, expectedValues));
	}
}

public class Aliases : Dictionary<string, PhysicalAddress>
{
	public Aliases() : base(StringComparer.OrdinalIgnoreCase) { }
}

public class Brightnesses : Dictionary<string, double>
{
	public Brightnesses() : base(StringComparer.OrdinalIgnoreCase) { }
}

public class Temperatures : Dictionary<string, short>
{
	public Temperatures() : base(StringComparer.OrdinalIgnoreCase) { }
}
