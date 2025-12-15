namespace ProyectoMejoramiento.Models
{
    public class Producto
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }

        public decimal CalcularTotal()
        {
            var total = PrecioUnitario * Cantidad; 
            return total;                          
        }
    }
}