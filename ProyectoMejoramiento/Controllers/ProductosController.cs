using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ProyectoMejoramiento.Data;
using ProyectoMejoramiento.Models;


namespace ProyectoMejoramiento.Controllers
{
    public class ProductosController : Controller
    {
        private readonly InventarioContexto _contexto;

        public ProductosController(InventarioContexto contexto)
        {
            _contexto = contexto;
        }

        // GET: /Productos
        public async Task<IActionResult> Index(string cadenaBusqueda)
        {
            var productos = from p in _contexto.Productos select p;

            if (!string.IsNullOrEmpty(cadenaBusqueda))
            {
                productos = productos.Where(p => p.Nombre.Contains(cadenaBusqueda));
            }

            return View(await productos.ToListAsync());
        }

        // GET: /Productos/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: /Productos/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([Bind("Nombre,CodigoDeBarras,Precio,Stock")] Producto producto)
        {

            if (ModelState.IsValid)
            {
                _contexto.Add(producto);
                await _contexto.SaveChangesAsync();
                TempData["MensajeExito"] = "Producto registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: /Productos/Editar/5
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _contexto.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        // POST: /Productos/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var productoExistente = await _contexto.Productos.FindAsync(id);
                    if (productoExistente == null)
                    {
                        return NotFound();
                    }

                    // Actualizar propiedades
                    productoExistente.Nombre = producto.Nombre;
                    productoExistente.CodigoDeBarras = producto.CodigoDeBarras;
                    productoExistente.Precio = producto.Precio;
                    productoExistente.Stock = producto.Stock;

                    await _contexto.SaveChangesAsync();
                    TempData["MensajeExito"] = "Producto actualizado correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExiste(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }


        private bool ProductoExiste(int id)
        {
            return _contexto.Productos.Any(e => e.Id == id);
        }
    }
}