using System.Text.Encodings.Web;

namespace Finance.Api.Domain
{
    public class ContaBancaria
    {
        public string Titular { get; }
        public decimal Saldo { get; private set; }
        public List<Transacao> Historico { get; } = new List<Transacao>();

        public ContaBancaria(string titular)
        {
            Titular = titular;
            Saldo = 0m;
        }

        public void Depositar(decimal valor)
        {
            Saldo += valor;

            Historico.Add(new Transacao("Depósito de: " + valor, valor, DateTime.Now));
        }

        public void Sacar(decimal valor)
        {
            if (valor > Saldo)
            {
                throw new InvalidOperationException("Saldo insuficiente");
            }

            Saldo -= valor;

            Historico.Add(new Transacao("Saque de: " + valor, valor, DateTime.Now));
        }

        public void Transferir(ContaBancaria contaDestino, decimal valor)
        {
            Saldo -= valor;

            contaDestino.Saldo += valor;

            Historico.Add(new Transacao(
                $"Transferência de R${valor} enviada para {contaDestino.Titular}.",
                valor,
                DateTime.Now
            ));

            contaDestino.Historico.Add(new Transacao(
                $"Transferência de R${valor} recebida de {Titular}", 
                valor, 
                DateTime.Now
            ));
        }

        public decimal SaldoConta()
        {
            return Saldo;
        }
    }
}
