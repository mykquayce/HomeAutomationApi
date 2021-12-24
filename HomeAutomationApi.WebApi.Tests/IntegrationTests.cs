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
	[InlineData("put", "/elgato/elgato/brightness/brightest", @"^{""on"":true,""brightness"":100,""temperature"":\d+}$")]
	[InlineData("put", "/elgato/elgato/brightness/brighter", @"^{""on"":true,""brightness"":83,""temperature"":\d+}$")]
	[InlineData("put", "/elgato/elgato/brightness/bright", @"^{""on"":true,""brightness"":67,""temperature"":\d+}$")]
	[InlineData("put", "/elgato/elgato/brightness/half", @"^{""on"":true,""brightness"":50,""temperature"":\d+}$")]
	[InlineData("put", "/elgato/elgato/brightness/dim", @"^{""on"":true,""brightness"":34,""temperature"":\d+}$")]
	[InlineData("put", "/elgato/elgato/brightness/dimmer", @"^{""on"":true,""brightness"":17,""temperature"":\d+}$")]
	[InlineData("put", "/elgato/elgato/brightness/dimmest", @"^{""on"":true,""brightness"":0,""temperature"":\d+}$")]
	public async Task Test1(string httpMethod, string relativeUriString, string expectedResponsePattern)
	{
		// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#basic-tests-with-the-default-webapplicationfactory
		string responseJson;
		{
			// Arrange
			await using var application = new WebApplicationFactory<Program>();
			var client = application.CreateClient();
			var request = new HttpRequestMessage(new HttpMethod(httpMethod), relativeUriString);

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
