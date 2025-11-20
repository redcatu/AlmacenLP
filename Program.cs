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
// 2. CONFIGURACIÓN DE BASE DE DATOS (LECTURA SIMPLE)
// -----------------------------------------------------------------------------
// .NET leerá la variable de entorno: ConnectionStrings__AlmacenLPContext
var connectionString = builder.Configuration.GetConnectionString("AlmacenLPContext") 
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'AlmacenLPContext'.");

builder.Services.AddDbContext<AlmacenLPContext>(options =>
    options.UseNpgsql(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))); // Uso de SplitQuery para optimizar, opcional

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
        // Si la migración falla, es mejor que el servicio no continúe 
        // para que Railway lo reinicie y muestre el error completo.
        // throw; // Se podría usar para forzar el fallo, pero con el Logger es suficiente para el diagnóstico.
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
