using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoBanco
{
    public class ContaCorrente : Conta
    {
        public static decimal TaxaManutencao { get; set; }
        private int transferenciasRealizadas;

        public ContaCorrente()
        {
            transferenciasRealizadas = 0;
            TaxaManutencao = 0.05m;
        }

        public override void Transferir(decimal quantia)
        {
            decimal valorTransferencia = quantia;

            transferenciasRealizadas++;

            if (transferenciasRealizadas % 2 == 0)
            {
                valorTransferencia = DescontarTaxa(quantia);
                Saldo -= valorTransferencia;
            }
            else
            {
                Saldo -= valorTransferencia;
            }

            Console.WriteLine($"Transferência realizada com sucesso\nSaldo atual: R${Saldo}");

            if (transferenciasRealizadas % 2 == 0) // Resetar contador após cada par de transferências
            {
                transferenciasRealizadas = 0;
            }
        }

        public override void Depositar(decimal quantia)
        {
            Saldo += quantia;
            Console.WriteLine($"Depósito realizado com sucesso\nSaldo atual: R${Saldo}");
        }

        internal static decimal DescontarTaxa(decimal quantiaTransferida)
        {
            decimal taxa = quantiaTransferida * TaxaManutencao; // 5% de taxa
            return taxa;
        }
    }
}

