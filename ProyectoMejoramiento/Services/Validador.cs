

namespace ProyectoMejoramiento.Services
{
    public class Validador
    {
        public bool EsEntero(string texto)
        {
            return int.TryParse(texto, out _);
        }
    }
}