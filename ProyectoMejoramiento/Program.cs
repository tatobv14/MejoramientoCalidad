using Microsoft.EntityFrameworkCore;
using ProyectoMejoramiento.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var cadenaConexion = builder.Configuration.GetConnectionString("ConexionInventario");

builder.Services.AddDbContext<InventarioContexto>(options =>
    options.UseMySql(cadenaConexion, ServerVersion.AutoDetect(cadenaConexion)));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Index}/{id?}");

app.Run();