using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using ServicioRNC.Server.Dal;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Contexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);
builder.Services.AddHttpClient();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 524288000;  // 500 MB
})
.ConfigureServices(services =>
{
    services.Configure<IISServerOptions>(options =>
    {
        // Establecer el l�mite m�ximo de tama�o de solicitud para IIS Express
        options.MaxRequestBodySize = 524288000;  // 500 MB
    });
});

var app = builder.Build();

// Configuraci�n de la canalizaci�n HTTP
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging(); // Para la depuraci�n en Blazor WebAssembly
}
else
{
    app.UseExceptionHandler("/Error"); // Manejador de excepciones en producci�n
    app.UseHsts(); // Seguridad HTTPS
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles(); // Habilita archivos est�ticos para Blazor WebAssembly
app.UseStaticFiles();
app.UseSwaggerUI(); // Interfaz de Swagger
app.UseRouting();

// Mapear las rutas para Razor Pages, Swagger y controladores
app.MapRazorPages();
app.UseSwagger(); // Habilitar Swagger
app.MapControllers();
app.MapFallbackToFile("index.html"); // Mapeo de archivo de entrada para Blazor

app.Run(); // Ejecuta la aplicaci�n
