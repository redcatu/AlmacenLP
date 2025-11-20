using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AlmacenLP.Infraestructura.Data;
using AlmacenLP.Core.Interfaces;
using AlmacenLP.Infraestructura.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------------
// CONFIGURACIÓN DE PUERTO PARA RAILWAY
// -----------------------------------------------------------------------------
// Railway asigna un puerto dinámico en la variable de entorno "PORT".
// Si no existe (localmente), usa el 8080.
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// -----------------------------------------------------------------------------
// CONFIGURACIÓN DE BASE DE DATOS
// -----------------------------------------------------------------------------
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
builder.Services.AddSwaggerGen(); // Registra el servicio, pero faltaba usarlo abajo
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
// PIPELINE DE LA APLICACIÓN
// -----------------------------------------------------------------------------

// IMPORTANTE: Habilitar Swagger también en producción para que puedas probarlo en Railway
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection(); // Correcto: Comentado para evitar loops de redirección en Railway

app.UseCors("myApp");

app.UseAuthorization();

app.MapControllers();

// Endpoint de salud para verificar rápidamente si la app levantó
app.MapGet("/", () => "API AlmacenLP funcionando correctamente! Ve a /swagger para ver la doc.");

app.Run();