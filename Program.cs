using System;
using System.Collections.Generic;
using ProjetoBanco;
using static ProjetoBanco.Cliente;
using static ProjetoBanco.Conta;

List<Cliente> clientes = new List<Cliente>();

Console.WriteLine("Sistema Banco Central iniciado.");
bool continuarPrograma = true;

do
{
    Console.WriteLine("\nO que você deseja realizar?:" +
        "\n1. Cadastrar nova conta." +
        "\n2. Transferir dinheiro." +
        "\n3. Depositar dinheiro." +
        "\n4. Consultar saldo." +
        "\n5. Listar Contas." +
        "\n6. Sair.");

    string opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            CadastrarContaCliente();
            break;
        case "2":
            TransferirDinheiro();
            break;
        case "3":
            DepositarDinheiro();
            break;
        case "4":
            ConsultarSaldo();
            break;
        case "5":
            ListarContas();
            break;
        case "6":
            Console.WriteLine("\nEncerrando Programa...");
            continuarPrograma = false;
            break;
        default:
            Console.WriteLine("\nOpção inválida. Tente Novamente.");
            break;
    }
} while (continuarPrograma);

void CadastrarContaCliente()
{
    Cliente cliente = new Cliente();
    Conta conta;

    Console.WriteLine("Insira o nome completo do cliente:");
    string nome = Console.ReadLine();

    Console.WriteLine("Insira a data de nascimento do cliente (dd/mm/aaaa):");
    DateTime dataNascimento;
    while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dataNascimento))
    {
        Console.WriteLine("Por favor, insira uma data válida no formato dd/mm/aaaa: ");
    }

    string cpf;
    bool cpfValido = false;
    do
    {
        Console.WriteLine("Insira o CPF do cliente (somente com números; deve conter 11 dígitos)");
        cpf = Console.ReadLine();
        if (cpf.Length != 11 || !long.TryParse(cpf, out _))
        {
            Console.WriteLine("CPF inválido. O CPF deve conter exatamente 11 dígitos numéricos. Tente novamente.\n");
        }
        else if (Cliente.CPFJaCadastrado(clientes, cpf))
        {
            Console.WriteLine("CPF já cadastrado. Não é possível cadastrar o mesmo CPF novamente. Tente novamente.\n");
        }
        else
        {
            cpfValido = true;
        }
    } while (!cpfValido);

    cliente.Cpf = cpf;

    Console.WriteLine("Insira o tipo de conta do cliente:\n0. Conta Corrente.\n1. Conta Poupança.");
    int tipoContaInt;
    while (!int.TryParse(Console.ReadLine(), out tipoContaInt) || tipoContaInt < 0 || tipoContaInt > 1)
    {
        Console.WriteLine("Opção inválida. Escolha 0 para Corrente ou 1 para Poupança.");
    }
    TipoConta tipoConta = (TipoConta)tipoContaInt;

    if (tipoConta == TipoConta.Corrente)
    {
        conta = new ContaCorrente();
    }
    else
    {
        conta = new ContaPoupanca();
    }

    cliente.Nome = nome;
    cliente.DataNascimento = dataNascimento;
    cliente.Cpf = cpf;
    cliente.Tipo = TipoCliente.Comum;
    cliente.Conta = conta;

    clientes.Add(cliente);

    Console.WriteLine("\nConta cadastrada com sucesso!");
}

void TransferirDinheiro()
{
    Console.WriteLine("\nInsira o CPF do cliente que vai fazer a transferência ou dígite '0' para cancelar:");
    string consultarCPFClienteTransferencia = Console.ReadLine();

    Cliente clienteTransferencia = BuscarCliente(consultarCPFClienteTransferencia);

    if (consultarCPFClienteTransferencia == "0")
    {
        Console.WriteLine("\nTransferência Cancelada!!");
        return;
    }
    else if (clienteTransferencia == null)
    {
        Console.WriteLine($"\nCliente com CPF '{consultarCPFClienteTransferencia}' não encontrado no sistema.");
        return;
    }

    Console.WriteLine("Insira o valor da transferência ou dígite '0' para cancelar:");
    decimal quantiaTransferencia;
    while (!decimal.TryParse(Console.ReadLine(), out quantiaTransferencia) || quantiaTransferencia < 0 || quantiaTransferencia > clienteTransferencia.Conta.Saldo)
    {
        Console.WriteLine($"\nValor inválido ou maior que o saldo disponível!" +
                          $"\nSaldo atual: R${clienteTransferencia.Conta.Saldo}" +
                          $"\nTente novamente ou digite '0' para cancelar: ");
    }
    if (quantiaTransferencia == 0)
    {
        Console.WriteLine("\nTransferência Cancelada!!");
        return;
    }

    if (clienteTransferencia.Conta is ContaCorrente)
    {
        decimal valorTransferencia = ContaCorrente.DescontarTaxa(quantiaTransferencia);
        clienteTransferencia.Conta.Transferir(valorTransferencia);
        clienteTransferencia.AtualizarTipoCliente();
    }
    else
    {
        clienteTransferencia.Conta.Transferir(quantiaTransferencia);
        clienteTransferencia.AtualizarTipoCliente();
    }
}

void DepositarDinheiro()
{
    Console.WriteLine("\nInsira o CPF do cliente que fará o depósito ou dígite '0' para cancelar:");
    string consultarCPFClienteDeposito = Console.ReadLine();

    Cliente clienteDeposito = BuscarCliente(consultarCPFClienteDeposito);

    if (consultarCPFClienteDeposito == "0")
    {
        Console.WriteLine("\nDepósito Cancelado!!");
        return;
    }
    else if (clienteDeposito == null)
    {
        Console.WriteLine($"\nCliente com CPF '{consultarCPFClienteDeposito}' não encontrado no sistema.");
        return;
    }

    Console.WriteLine($"Insira o valor a ser depositado ou dígite '0' para cancelar:");
    decimal quantiaDeposito;
    while (!decimal.TryParse(Console.ReadLine(), out quantiaDeposito) || quantiaDeposito < 0)
    {
        Console.WriteLine("Valor inválido! Insira um valor válido ou dígite '0' para cancelar: ");
    }
    if (quantiaDeposito == 0)
    {
        Console.WriteLine("\nDepósito Cancelado!!");
        return;
    }

    if (clienteDeposito.Conta is ContaPoupanca)
    {
        decimal valorDepositado = ContaPoupanca.AcrescentarRendimento(quantiaDeposito);
        clienteDeposito.Conta.Depositar(valorDepositado);
        clienteDeposito.AtualizarTipoCliente();
    }
    else
    {
        clienteDeposito.Conta.Depositar(quantiaDeposito);
        clienteDeposito.AtualizarTipoCliente();
    }
}

void ConsultarSaldo()
{
    Console.WriteLine("\nInsira o CPF do cliente que deseja consultar: ");
    string consultarCPFCliente = Console.ReadLine();

    Cliente cliente = BuscarCliente(consultarCPFCliente);

    if (cliente != null)
    {
        Console.WriteLine($"\nDADOS DO CLIENTE." +
            $"\nNome: {cliente.Nome}" +
            $"\nCPF: {FormatarCPF(cliente.Cpf)}" +
            $"\nNível: {cliente.Tipo}" +
            $"\n\nDADOS DA CONTA." +
            $"\nNúmero: {cliente.Conta.Numero}" +
            $"\nSaldo: R${cliente.Conta.Saldo}" +
            $"\nTipo de conta: {FormatarConta(cliente.Conta.GetType().Name)}");
    }
    else
    {
        Console.WriteLine($"\nCliente com CPF '{consultarCPFCliente}' não encontrado no sistema.");
    }
}

Cliente BuscarCliente(string cpf)
{
    foreach (Cliente cliente in clientes)
    {
        if (cliente.Cpf.Equals(cpf, StringComparison.OrdinalIgnoreCase))
        {
            return cliente;
        }
    }
    return null;
}

string FormatarCPF(string cpf)
{
    return cpf.Insert(3, ".").Insert(7, ".").Insert(11, "-");
}
string FormatarConta(string conta)
{
    return conta.Replace("Conta", "");
}
void ListarContas()
{
    if (clientes.Count == 0)
    {
        Console.WriteLine("\nNenhuma conta cadastrada!");
    }
    else
    {
        Console.WriteLine("\nContas:\n");
        for (int i = 0; i < clientes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. - Nome: {clientes[i].Nome}" +
                $" - Id Conta: {clientes[i].Conta.ContaId}" +
                $" - Numero conta: {clientes[i].Conta.Numero}" +
                $" - Tipo Conta: {FormatarConta(clientes[i].Conta.GetType().Name)}" +
                $" - Saldo: R${clientes[i].Conta.Saldo}");
        }
    }
}
