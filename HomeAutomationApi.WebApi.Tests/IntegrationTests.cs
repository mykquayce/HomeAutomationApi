using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace HomeAutomationApi.WebApi.Tests;

public class IntegrationTests
{
	[Theory]
	[InlineData("get", "/elgato/elgato", @"^{""on"":(?:false|true),""brightness"":\d+,""temperature"":\d+}$")]
	[InlineData("put", "/elgato/elgato/power/off", @"^{""on"":false,""brightness"":\d+,""temperature"":\d+}$")]
	[InlineData("put", "/elgato/elgato/power/on", @"^{""on"":true,""brightness"":\d+,""temperature"":\d+}$")]
	[InlineData("put", "/elgato/elgato/power/toggle", @"^{""on"":(?:false|true),""brightness"":\d+,""temperature"":\d+}$")]
	public async Task Test1(string method, string relativeUriString, string expectedResponsePattern)
	{
		// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#basic-tests-with-the-default-webapplicationfactory
		string responseJson;
		{
			// Arrange
			var httpMethod = method switch
			{
				"get" => HttpMethod.Get,
				"put" => HttpMethod.Put,
			};
			await using var application = new WebApplicationFactory<Program>();
			var client = application.CreateClient();
			var request = new HttpRequestMessage(httpMethod, relativeUriString);

			// Act
			var response = await client.SendAsync(request);

			// Assert
			Assert.True(response.IsSuccessStatusCode);
			responseJson = await response.Content.ReadAsStringAsync();
		}

		// Assert
		Assert.NotNull(responseJson);
		Assert.NotEmpty(responseJson);
		Assert.StartsWith("{", responseJson);
		Assert.Matches(expectedResponsePattern, responseJson);
	}
}
