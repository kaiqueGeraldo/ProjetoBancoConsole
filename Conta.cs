using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoBanco
{
    public abstract class Conta
    {
        private static int proximoIDConta = 1;
        public int ContaId { get; set; }
        private static int proximoNumeroConta = 1;
        public string Numero { get; set; }
        public decimal Saldo { get; set; }
        public TipoConta Tipo;

        public enum TipoConta
        {
            Corrente,
            Poupança
        }

        public abstract void Transferir(decimal quantia);
        public abstract void Depositar(decimal quantia);

        public Conta()
        {
            ContaId = proximoIDConta;
            proximoIDConta++;
            Numero = $"{proximoNumeroConta.ToString("0000")}";
            proximoNumeroConta++;
            Saldo = 0;
        }
    }
}