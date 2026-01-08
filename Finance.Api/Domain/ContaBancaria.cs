using System.Text.Encodings.Web;

namespace Finance.Api.Domain
{
    public class ContaBancaria
    {
        public string Titular { get; }
        public decimal Saldo { get; private set; }

        public ContaBancaria(string titular)
        {
            Titular = titular;
            Saldo = 0m;
        }

        public void Depositar(decimal valor)
        {
            Saldo += valor;
        }

        public void Sacar(decimal valor)
        {
            if (valor > Saldo)
                throw new InvalidOperationException("Saldo insuficiente");

            Saldo -= valor;
        }
    }
}
