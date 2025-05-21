using F1_Web.Data;
using F1_Web.EndPoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using F1_Web.Ergast;
using F1_Web.Model;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApiDocument(config =>
	{
		config.Title = "F1 API";
		config.DocumentName = "F1 API";
		config.Version = "v1";
	}
);

if (builder.Environment.IsDevelopment())
{
	builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

// Accediamo alla stringa di connessione
var connectionString = builder.Configuration.GetConnectionString("F1ApiConnection");
var serverVersion = ServerVersion.AutoDetect(connectionString);
//Aggiungiamo il servizio database tramite EF Core con MariaDB
builder.Services.AddDbContext<F1DbContext>(
	opt => opt.UseMySql(connectionString, serverVersion)
	.LogTo(Console.WriteLine, LogLevel.Information)
	.EnableSensitiveDataLogging()
	.EnableDetailedErrors()
	);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
	//permette di ottenere una rotta dove vedere la documentazione delle API secondo lo standard OpenAPI
	app.MapOpenApi();
	//permette a Swagger (NSwag) di generare un file JSON con le specifiche delle API
	app.UseOpenApi();
	//permette di configurare l'interfaccia SwaggerUI (l'interfaccia grafica web di Swagger (NSwag) che permette di interagire con le API)
	app.UseSwaggerUi(config =>
	{
		config.DocumentTitle = "F1 API";
		config.Path = "/swagger";
		config.DocumentPath = "/swagger/{documentName}/swagger.json";
		config.DocExpansion = "list";
	});
	app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();
// Configure Swagger UI before static files
app.UseSwaggerUi(config =>
{
	config.DocumentTitle = "F1 API";
	config.Path = "/swagger";
	config.DocumentPath = "/swagger/{documentName}/swagger.json";
	config.DocExpansion = "list";
});

// Serve static files after Swagger

// Middleware per file statici

// 1. Configura il middleware per servire index.html dalla cartella pages alla root
app.UseDefaultFiles(new DefaultFilesOptions
{
	FileProvider = new PhysicalFileProvider(
		Path.Combine(builder.Environment.WebRootPath, "pages")
	),
	RequestPath = ""
});

// // 2. Middleware per file statici in wwwroot (CSS, JS, ecc.)
app.UseStaticFiles();

// // 3. Middleware di fallback per cercare file in wwwroot/pages
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(
		Path.Combine(builder.Environment.WebRootPath, "pages")
	)
});

// routing per le API
//--------------------Endpoints management--------------------
app
.MapGroup("/api")
.MapCircuitoEndPoints()
.MapPilotaEndPoints()
.MapTeamEndPoints()
.WithOpenApi()
.WithTags("Public API");

//--------------------Endpoints management--------------------

app.Run();
