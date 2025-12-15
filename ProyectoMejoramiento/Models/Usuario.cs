namespace ProyectoMejoramiento.Models
{
    public class Usuario
    {
        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; }

        public bool PuedeIniciarSesion()
        {
            return Activo;
        }
    }
}