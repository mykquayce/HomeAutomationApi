using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace HomeAutomationApi.WebApplication.Tests;

public class UnitTest1
{
	[Theory]
	//[InlineData("/tplink/vr%20front/on")]
	[InlineData("/philipshue/wall%20right/off")]
	[InlineData("/philipshue/wall%20right/on")]
	[InlineData("/Elgato/keylight/toggle")]
	[InlineData("/Elgato/lightstrip/toggle")]
	public async Task Test1(string requestUri)
	{
		string body;
		HttpStatusCode status;
		{
			// Arrange
			using WebApplicationFactory<Program> _factory = new();
			using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, });

			// Act
			using var response = await client.PutAsync(requestUri, null);
			status = response.StatusCode;
			body = await response.Content.ReadAsStringAsync();
			// Don't dispose right away
			await Task.Delay(millisecondsDelay: 2_000);
		}

		// Assert
		Assert.True(HttpStatusCode.OK == status, userMessage: body);
	}
}
