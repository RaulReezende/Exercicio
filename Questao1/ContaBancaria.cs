using System;

namespace Questao1;

public class ContaBancaria 
{
    private readonly int _numeroConta;
    private string _nomeTitular;
    private double _saldo;
    private const double TaxaSaque = 3.50; 

    public int NumeroConta => _numeroConta;
    public string NomeTitular
    {
        get => _nomeTitular;
        set => _nomeTitular = !string.IsNullOrWhiteSpace(value) 
            ? value
            : throw new ArgumentException("Nome do titular não pode ser vazio.");
        
    }

    public double Saldo => _saldo;

    public ContaBancaria(int numeroConta, string titular, double depositoInicial = 0)
    {
        if(numeroConta <= 0)
            throw new ArgumentException("Número da conta deve ser positivo.");

        if(string.IsNullOrWhiteSpace(titular))
            throw new ArgumentException("Nome do titular não pode ser vazio.");

        if(depositoInicial < 0)
            throw new ArgumentException("Depósito inicial não pode ser negativo.");

        _numeroConta = numeroConta;
        _nomeTitular = titular;
        _saldo = depositoInicial;
    }

    public void Deposito(double quantia)
    {
        if(quantia <= 0)
            throw new ArgumentException("Valor do depósito deve ser positivo.");

        _saldo += quantia;
    }

    public void Saque(double quantia)
    {
        if(quantia <= 0)
            throw new ArgumentException("Valor do saque deve ser positivo.");

        _saldo -= quantia + TaxaSaque;
    }

    public override string ToString()
    {
        return $"Conta {NumeroConta}, Titular: {NomeTitular}, Saldo: $ {Saldo:F2}";
    }

}
