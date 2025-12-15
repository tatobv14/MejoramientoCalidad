using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoMejoramiento.Controllers;
using ProyectoMejoramiento.Data;
using ProyectoMejoramiento.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;


public class ProductosControllerTests
{
    private InventarioContexto GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<InventarioContexto>()
            .UseInMemoryDatabase(databaseName: "InventarioTestDb")
            .Options;
        var context = new InventarioContexto(options);

        // Seed data
        if (!context.Productos.Any())
        {
            context.Productos.AddRange(
                new Producto { Nombre = "Producto A", CodigoDeBarras = "111", Precio = 10, Stock = 20 },
                new Producto { Nombre = "Producto B", CodigoDeBarras = "222", Precio = 15, Stock = 3 },
                new Producto { Nombre = "Jugo Natural", CodigoDeBarras = "333", Precio = 5, Stock = 10 }
            );
            context.SaveChanges();
        }

        return context;
    }

    [Fact]
    public async Task Index_ReturnsAllProducts_WhenNoSearch()
    {
        // Arrange
        var context = GetInMemoryContext();
        var controller = new ProductosController(context);

        // Act
        var result = await controller.Index(null) as ViewResult;
        var model = result.Model as IQueryable<Producto> ?? (result.Model as System.Collections.Generic.IEnumerable<Producto>).AsQueryable();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, model.Count());
    }

    [Fact]
    public async Task Index_FiltersProducts_BySearchTerm()
    {
        // Arrange
        var context = GetInMemoryContext();
        var controller = new ProductosController(context);

        // Act
        var result = await controller.Index("Jugo") as ViewResult;
        var model = result.Model as IQueryable<Producto> ?? (result.Model as System.Collections.Generic.IEnumerable<Producto>).AsQueryable();

        // Assert
        Assert.NotNull(result);
        Assert.Single(model);
        Assert.Contains(model, p => p.Nombre.Contains("Jugo"));
    }

    [Fact]
    public async Task Crear_Post_ValidProduct_RedirectsToIndex()
    {
        // Arrange
        var context = GetInMemoryContext();
        var controller = new ProductosController(context);
        var nuevoProducto = new Producto { Nombre = "Nuevo Producto", CodigoDeBarras = "999", Precio = 20, Stock = 5 };

        controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

        // Act
        var result = await controller.Crear(nuevoProducto);

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
        Assert.Contains(context.Productos, p => p.Nombre == "Nuevo Producto");
    }

    [Fact]
    public async Task Crear_Post_InvalidModel_ReturnsViewWithModel()
    {
        // Arrange
        var context = GetInMemoryContext();
        var controller = new ProductosController(context);
        controller.ModelState.AddModelError("Nombre", "Required");
        var productoInvalido = new Producto();

        // Act
        var result = await controller.Crear(productoInvalido);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(productoInvalido, viewResult.Model);
    }

    [Fact]
    public async Task Editar_Get_ExistingId_ReturnsViewWithProduct()
    {
        // Arrange
        var context = GetInMemoryContext();
        var controller = new ProductosController(context);
        var productoExistente = context.Productos.First();

        // Act
        var result = await controller.Editar(productoExistente.Id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Producto>(viewResult.Model);
        Assert.Equal(productoExistente.Id, model.Id);
    }

    [Fact]
    public async Task Editar_Get_NullId_ReturnsNotFound()
    {
        // Arrange
        var context = GetInMemoryContext();
        var controller = new ProductosController(context);

        // Act
        var result = await controller.Editar(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Editar_Post_ValidProduct_RedirectsToIndex()
    {
        // Arrange
        var context = GetInMemoryContext();
        var controller = new ProductosController(context);
        var productoExistente = context.Productos.First();
        productoExistente.Nombre = "Nombre Modificado";
        controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

        // Act
        var result = await controller.Editar(productoExistente.Id, productoExistente);

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
        var productoModificado = context.Productos.Find(productoExistente.Id);
        Assert.Equal("Nombre Modificado", productoModificado.Nombre);
    }

    [Fact]
    public async Task Editar_Post_InvalidModel_ReturnsViewWithModel()
    {
        // Arrange
        var context = GetInMemoryContext();
        var controller = new ProductosController(context);
        var productoExistente = context.Productos.First();
        controller.ModelState.AddModelError("Nombre", "Required");

        // Act
        var result = await controller.Editar(productoExistente.Id, productoExistente);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(productoExistente, viewResult.Model);
    }
}