using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace HomeAutomationApi.WebApi.Tests;

public class IntegrationTests
{
	[Theory]
	[InlineData("/elgato/elgato", @"^{""on"":(?:false|true),""brightness"":\d+,""temperature"":\d+}$")]
	public async Task Test1(string relativeUriString, string expectedResponsePattern)
	{
		// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#basic-tests-with-the-default-webapplicationfactory
		string response;
		{
			// Arrange
			await using var application = new WebApplicationFactory<Program>();
			var client = application.CreateClient();

			// Act
			response = await client.GetStringAsync(relativeUriString);
		}

		// Assert
		Assert.NotNull(response);
		Assert.NotEmpty(response);
		Assert.StartsWith("{", response);
		Assert.Matches(expectedResponsePattern, response);
	}
}
