using Xunit;
using ProyectoMejoramiento.Models;

public class ProductoTests
{
    [Fact]
    public void CalcularTotal_CantidadPorPrecio_RetornaCorrecto()
    {
        var producto = new Producto { Nombre = "Lapicero", PrecioUnitario = 2m, Cantidad = 5 };
        var total = producto.CalcularTotal();
        Assert.Equal(10m, total);
    }
}