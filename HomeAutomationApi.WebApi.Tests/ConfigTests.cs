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
}

public class Aliases : Dictionary<string, PhysicalAddress> { }
