namespace Questao5.Domain.Models;

public class SaldoContaResponse
{
    public int NumeroConta { get; set; }
    public string NomeTitular { get; set; }
    public DateTime DataConsulta { get; set; }
    public decimal Saldo { get; set; }
}
