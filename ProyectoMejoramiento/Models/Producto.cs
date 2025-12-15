namespace ProyectoMejoramiento.Models
{
    public class Producto
    {
        public string Nombre { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }

        public decimal CalcularTotal()
        {
            return PrecioUnitario * Cantidad;
        }
    }
}