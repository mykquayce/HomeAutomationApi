using Microsoft.AspNetCore.Mvc.Testing;

namespace HomeAutomationApi.WebApplication.Tests;

public class UnitTest1
{
	[Theory]
	[InlineData("/weatherforecast")]
	public async Task Test1(string requestUri)
	{
		string json;
		{
			// Arrange
			using WebApplicationFactory<Program> _factory = new();
			using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, });

			// Act
			json = await client.GetStringAsync(requestUri);
		}

		// Assert
		Assert.NotNull(json);
		Assert.NotEmpty(json);
		Assert.StartsWith("[", json);
		Assert.NotEqual("[]", json);
	}
}
