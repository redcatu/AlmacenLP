using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AlmacenLP.Infraestructura.Data;
using AlmacenLP.Core.Interfaces;
using AlmacenLP.Infraestructura.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------------
// CONFIGURACIÓN DE PUERTO PARA RAILWAY
// -----------------------------------------------------------------------------
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// -----------------------------------------------------------------------------
// CONFIGURACIÓN DE BASE DE DATOS
// -----------------------------------------------------------------------------
// La variable de entorno debe llamarse: ConnectionStrings__AlmacenLPContext
builder.Services.AddDbContext<AlmacenLPContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AlmacenLPContext") 
    ?? throw new InvalidOperationException("Connection string 'AlmacenLPContext' not found.")));

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
// SERVICIOS Y CONTROLADORES
// -----------------------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// -----------------------------------------------------------------------------
// INYECCIÓN DE DEPENDENCIAS (REPOSITORIOS)
// -----------------------------------------------------------------------------
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
builder.Services.AddScoped<ICargaRepositorio, CargaRepositorio>();
builder.Services.AddScoped<ICamionRepositorio, CamionRepositorio>();
builder.Services.AddScoped<IAlmacenRepositorio, AlmacenRepositorio>();
builder.Services.AddScoped<IBajaProductoRepositorio, BajaProductoRepositorio>();
builder.Services.AddScoped<IInventarioRepositorio, InventarioRepositorio>();
builder.Services.AddScoped<ILoteRepositorio, LoteRepositorio>();

var app = builder.Build();

// -----------------------------------------------------------------------------
// MIGRACIÓN AUTOMÁTICA DE BASE DE DATOS
// -----------------------------------------------------------------------------
// Esto asegura que la BD se cree y tenga las tablas al iniciar en Railway
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AlmacenLPContext>();
        // Aplica cualquier migración pendiente (crea la DB si no existe)
        context.Database.Migrate();
        Console.WriteLine("--> Migraciones aplicadas correctamente.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "--> Ocurrió un error al migrar la base de datos.");
    }
}

// -----------------------------------------------------------------------------
// PIPELINE DE LA APLICACIÓN
// -----------------------------------------------------------------------------

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection(); 

app.UseCors("myApp");

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "API AlmacenLP funcionando y conectada a DB!");

app.Run();
