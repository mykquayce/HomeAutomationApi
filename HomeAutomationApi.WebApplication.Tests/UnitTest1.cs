using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace HomeAutomationApi.WebApplication.Tests;

public class UnitTest1
{
	[Theory]
	[InlineData("/tplink/vr%20front/on")]
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
		}

		// Assert
		Assert.True(HttpStatusCode.OK == status, userMessage: body);
	}
}
