using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AlmacenLP.Infraestructura.Data;
using AlmacenLP.Core.Interfaces;
using AlmacenLP.Infraestructura.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// Puerto dinámico para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Configuración de la DB
builder.Services.AddDbContext<AlmacenLPContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AlmacenLPContext") 
    ?? throw new InvalidOperationException("Connection string 'AlmacenLPContext' not found.")));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("myApp", policibuilder =>
    {
        policibuilder.AllowAnyOrigin();
        policibuilder.AllowAnyHeader();
        policibuilder.AllowAnyMethod();
    });
});

// Servicios y controladores
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

// app.UseHttpsRedirection(); // No es necesario en Railway

app.UseCors("myApp");
app.UseAuthorization();
app.MapControllers();

// Endpoint de prueba para verificar que la app funciona
app.MapGet("/", () => "API AlmacenLP funcionando!");

app.Run();
