using Microsoft.EntityFrameworkCore;

namespace ProyectoMejoramiento.Data
{
    public class InventarioContexto : DbContext
    {
        public InventarioContexto(DbContextOptions<InventarioContexto> opciones) : base(opciones) { }

        public DbSet<Models.Producto> Productos { get; set; }
    }
}