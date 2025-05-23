using Questao1.Services;
using System;
using System.Globalization;

namespace Questao1;

class Program {
    static void Main(string[] args) {

        var contaService = new ContaBancariaService();

        try
        {
            var account = contaService.CriarConta();
            contaService.RealizarOperacao(account);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }

        /* Output expected:
        Exemplo 1:

        Entre o número da conta: 5447
        Entre o titular da conta: Milton Gonçalves
        Haverá depósito inicial(s / n) ? s
        Entre o valor de depósito inicial: 350.00

        Dados da conta:
        Conta 5447, Titular: Milton Gonçalves, Saldo: $ 350.00

        Entre um valor para depósito: 200
        Dados da conta atualizados:
        Conta 5447, Titular: Milton Gonçalves, Saldo: $ 550.00

        Entre um valor para saque: 199
        Dados da conta atualizados:
        Conta 5447, Titular: Milton Gonçalves, Saldo: $ 347.50

        Exemplo 2:
        Entre o número da conta: 5139
        Entre o titular da conta: Elza Soares
        Haverá depósito inicial(s / n) ? n

        Dados da conta:
        Conta 5139, Titular: Elza Soares, Saldo: $ 0.00

        Entre um valor para depósito: 300.00
        Dados da conta atualizados:
        Conta 5139, Titular: Elza Soares, Saldo: $ 300.00

        Entre um valor para saque: 298.00
        Dados da conta atualizados:
        Conta 5139, Titular: Elza Soares, Saldo: $ -1.50
        */
    }
}
