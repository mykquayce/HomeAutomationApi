using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.AddNetworkDiscovery(builder.Configuration.GetSection("identity"), builder.Configuration.GetSection("networkdiscovery"))
	.AddElgato(builder.Configuration.GetSection("elgato"))
	.AddPhilipsHue(builder.Configuration.GetSection("philipshue"), provider =>
	{
		var disco = provider.GetRequiredService<Helpers.NetworkDiscovery.IClient>();
		(_, _, IPAddress ip, _, _) = disco.ResolveAsync("philipshue").GetAwaiter().GetResult();
		var uri = new UriBuilder("http", ip.ToString()).Uri;
		return uri;
	})
	.AddTPLink(Helpers.TPLink.Config.Defaults)
	.AddTransient<HomeAutomationApi.Services.IElgatoService, HomeAutomationApi.Services.Concrete.ElgatoService>()
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
