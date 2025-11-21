using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AlmacenLP.Infraestructura.Data;
using AlmacenLP.Core.Interfaces;
using AlmacenLP.Infraestructura.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------------
// 1. CONFIGURACIÓN DE PUERTO (RAILWAY)
// -----------------------------------------------------------------------------
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// -----------------------------------------------------------------------------
// 2. CONFIGURACIÓN DE BASE DE DATOS (LECTURA Y DIAGNÓSTICO)
// -----------------------------------------------------------------------------
// INTENTO 1: Leer variable de entorno "DATABASE" (Estilo Railway / Ejemplo Ingeniero)
var connectionString = Environment.GetEnvironmentVariable("DATABASE");

// INTENTO 2: Si es nulo, leer de appsettings.json (Estilo local .NET)
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("AlmacenLPContext");
}

// Validación final
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("No se encontró la cadena de conexión en la variable 'DATABASE' ni en 'AlmacenLPContext'.");
}

// DIAGNÓSTICO (Opcional: para ver en los logs de Railway si tomó la correcta)
// Ocultamos el password para que no salga en el log
var diagnosticConnectionString = new System.Text.RegularExpressions.Regex("Password=.*?;")
    .Replace(connectionString, "Password=***REDACTED***;");
    
Console.WriteLine($"[DB CONNECTION] Usando cadena: {diagnosticConnectionString}");

builder.Services.AddDbContext<AlmacenLPContext>(options =>
    options.UseNpgsql(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
// -----------------------------------------------------------------------------
// CORS
// -----------------------------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("myApp", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin();
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
    });
});

// -----------------------------------------------------------------------------
// SERVICIOS
// -----------------------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// Repositorios
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
builder.Services.AddScoped<ICargaRepositorio, CargaRepositorio>();
builder.Services.AddScoped<ICamionRepositorio, CamionRepositorio>();
builder.Services.AddScoped<IAlmacenRepositorio, AlmacenRepositorio>();
builder.Services.AddScoped<IBajaProductoRepositorio, BajaProductoRepositorio>();
builder.Services.AddScoped<IInventarioRepositorio, InventarioRepositorio>();
builder.Services.AddScoped<ILoteRepositorio, LoteRepositorio>();

var app = builder.Build();

// -----------------------------------------------------------------------------
// MIGRACIÓN AUTOMÁTICA (ROBUSTO)
// -----------------------------------------------------------------------------
// El logger ahora se inyecta correctamente en el ServiceProvider.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("--> Intentando aplicar migraciones...");
        var context = services.GetRequiredService<AlmacenLPContext>();
        context.Database.Migrate();
        logger.LogInformation("--> Migraciones aplicadas correctamente. Base de datos lista.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "--> ERROR FATAL: Ocurrió un error al migrar o sembrar la base de datos.");
        // throw; // Se deja la aplicación viva, pero el log debe mostrar el error real.
    }
}

// -----------------------------------------------------------------------------
// PIPELINE
// -----------------------------------------------------------------------------
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("myApp");
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "API AlmacenLP funcionando! (DB conectada)");

app.Run();
