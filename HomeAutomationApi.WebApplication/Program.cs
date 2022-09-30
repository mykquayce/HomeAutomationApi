using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.AddNetworkDiscoveryApi(builder.Configuration)
	.AddPhilipsHue(provider =>
	{
		var config = new Helpers.PhilipsHue.Config();
		builder.Configuration.GetSection("philipshue").Bind(config);

		if (config.Host is not null)
		{
			return config;
		}

		var physicalAddressString = builder.Configuration.GetSection("philipshue")["physicalAddress"];
		var physicalAddress = System.Net.NetworkInformation.PhysicalAddress.Parse(physicalAddressString);

		var resolver = provider.GetRequiredService<Helpers.NetworkDiscoveryApi.Delegates.ResolvePhysicalAddress>();

		(_, _, var ip, _, _) = resolver(physicalAddress);

		var host = new Uri("http://" + ip.ToString());

		return config with { Host = host, };
	})
	.AddTPLink(Helpers.TPLink.Config.Defaults)
	.AddTransient<HomeAutomationApi.Services.IPhilipsHueService, HomeAutomationApi.Services.Concrete.PhilipsHueService>()
	.AddTransient<HomeAutomationApi.Services.ITPLinkService, HomeAutomationApi.Services.Concrete.TPLinkService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

internal partial class Program { }
