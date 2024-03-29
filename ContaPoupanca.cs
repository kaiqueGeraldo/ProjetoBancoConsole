using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoBanco
{
    public class ContaPoupanca : Conta
    {
        public static decimal TaxaRendimento { get; set; }
        private int depositosRealizados;

        public ContaPoupanca()
        {
            depositosRealizados = 0;
            TaxaRendimento = 0.05m;
        }

        public override void Transferir(decimal quantia)
        {
            Saldo -= quantia;
            Console.WriteLine($"Transferência realizada com sucesso\nSaldo atual: R${Saldo}");
        }

        public override void Depositar(decimal quantia)
        {
            Saldo += quantia;
            depositosRealizados++;

            if (depositosRealizados % 2 == 0)
            {
                Saldo = Saldo + AcrescentarRendimento(Saldo);
            }

            Console.WriteLine($"Depósito realizado com sucesso\nSaldo atual: R${Saldo}");

            if (depositosRealizados % 2 == 0) // Resetar contador após cada par de depósitos
            {
                depositosRealizados = 0;
            }
        }

        internal static decimal AcrescentarRendimento(decimal saldo)
        {
            decimal rendimento = saldo * TaxaRendimento; // 5% de rendimento
            return rendimento;
        }
    }
}
