using System;

namespace ProjetoBanco
{
    public class Cliente
    {
        private static int proximoID = 1;
        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public TipoCliente Tipo { get; set; }
        public Conta Conta { get; set; }

        public enum TipoCliente
        {
            Comum,
            Super,
            Premium
        }

        public Cliente()
        {
            Id = proximoID;
            proximoID++;
            Tipo = TipoCliente.Comum;
        }

        public void AtualizarTipoCliente()
        {
            decimal saldo = Conta.Saldo;
            if (saldo >= 15000)
            {
                Tipo = TipoCliente.Premium;
            }
            else if (saldo >= 5000 && saldo < 15000)
            {
                Tipo = TipoCliente.Super;
            }
            else
            {
                Tipo = TipoCliente.Comum;
            }
        }
        public static bool CPFJaCadastrado(List<Cliente> clientes, string cpf)
        {
            foreach (Cliente cliente in clientes)
            {
                if (cliente.Cpf.Equals(cpf))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
