using Xunit;
using ProyectoMejoramiento.Models;

public class UsuarioTests
{
    [Fact]
    public void PuedeIniciarSesion_UsuarioActivo_RetornaTrue()
    {
        var usuario = new Usuario { Nombre = "Tatiana", Activo = true };
        Assert.True(usuario.PuedeIniciarSesion());
    }

    [Fact]
    public void PuedeIniciarSesion_UsuarioInactivo_RetornaFalse()
    {
        var usuario = new Usuario { Nombre = "Tatiana", Activo = false };
        Assert.False(usuario.PuedeIniciarSesion());
    }
}