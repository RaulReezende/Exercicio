using System;

namespace Questao1.Services;

public class ContaBancariaService
{
    public ContaBancaria CriarConta()
    {
        Console.Write("Entre o número da conta: ");
        int numero = int.Parse(Console.ReadLine());

        Console.Write("Entre o titular da conta: ");
        string titular = Console.ReadLine();

        Console.Write("Haverá depósito inicial (s/n)? ");
        var hasInitialDeposit = Console.ReadLine().ToLower() == "s";

        double initialDeposit = 0;
        if (hasInitialDeposit)
        {
            Console.Write("Entre o valor de depósito inicial: ");
            initialDeposit = double.Parse(Console.ReadLine());
        }

        return new ContaBancaria(numero, titular, initialDeposit);
    }

    public void RealizarOperacao(ContaBancaria conta)
    {
        Console.WriteLine("\nDados da conta:");
        Console.WriteLine(conta);

        Console.Write("\nEntre um valor para depósito: ");
        var depositoValor = double.Parse(Console.ReadLine());
        conta.Deposito(depositoValor);
        Console.WriteLine("Dados da conta atualizados:");
        Console.WriteLine(conta);

        Console.Write("\nEntre um valor para saque: ");
        var saqueValor = double.Parse(Console.ReadLine());
        conta.Saque(saqueValor);
        Console.WriteLine("Dados da conta atualizados:");
        Console.WriteLine(conta);
    }
}
