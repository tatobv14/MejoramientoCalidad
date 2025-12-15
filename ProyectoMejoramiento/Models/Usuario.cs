namespace ProyectoMejoramiento.Models
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        public bool PuedeIniciarSesion()
        {
            return Activo;
        }
    }
}