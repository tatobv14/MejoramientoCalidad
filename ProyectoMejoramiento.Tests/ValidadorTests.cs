using Xunit;
using ProyectoMejoramiento.Services;

public class ValidadorTests
{
    [Fact]
    public void EsEntero_TextoConNumeros_RetornaTrue()
    {
        var validador = new Validador();
        var resultado = validador.EsEntero("123");
        Assert.True(resultado);
    }

    [Fact]
    public void EsEntero_TextoConLetras_RetornaFalse()
    {
        var validador = new Validador();
        var resultado = validador.EsEntero("abc");
        Assert.False(resultado);
    }
}