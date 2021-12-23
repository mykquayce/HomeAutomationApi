var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.JsonConfig<HomeAutomationApi.WebApi.Models.AliasesLookup>(builder.Configuration.GetSection(nameof(HomeAutomationApi.WebApi.Models.AliasesLookup)));

builder.Services
	.AddHttpClient<Helpers.NetworkDiscoveryApi.IClient, Helpers.NetworkDiscoveryApi.Concrete.Client>(config =>
	{
		var baseAddress = builder.Configuration["NetworkDiscoveryApi:BaseAddress"];
		config.BaseAddress = new Uri(baseAddress);
	});

builder.Services
	.Configure<Helpers.Elgato.Concrete.ElgatoClient.Config>(builder.Configuration.GetSection(nameof(Helpers.Elgato)))
	.AddTransient<Helpers.Elgato.IElgatoClient, Helpers.Elgato.Concrete.ElgatoClient>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();

internal partial class Program { }
