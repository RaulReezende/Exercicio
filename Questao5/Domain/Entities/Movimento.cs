namespace Questao5.Domain.Entities;

public class Movimento
{
    public string IdMovimento { get; private set; }
    public string IdContaCorrente { get; private set; }
    public DateTime DataMovimento { get; private set; }
    public char TipoMovimento { get; private set; }
    public decimal Valor { get; private set; }

    public Movimento(string idMovimento, string idContaCorrente, DateTime dataMovimento, char tipoMovimento, decimal valor)
    {
        IdMovimento = idMovimento;
        IdContaCorrente = idContaCorrente;
        DataMovimento = dataMovimento;
        TipoMovimento = tipoMovimento;
        Valor = valor;
    }
}
