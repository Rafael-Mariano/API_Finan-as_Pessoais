using System.Text.Encodings.Web;

namespace Finance.Api.Domain
{
    public class ContaMovimento
    {
        public string NomeContaMovimento { get; }
        public decimal Saldo { get; private set; }
        public List<Transacao> Historico { get; } = new List<Transacao>();

        public ContaMovimento(string nomeConta)
        {
            NomeContaMovimento = nomeConta;
            Saldo = 0m;
        }

        public void Entrada(decimal valor)
        {
            Saldo += valor;

            Historico.Add(new Transacao("Entrada de: " + valor, valor, DateTime.Now));
        }

        public void Saida(decimal valor)
        {
            if (valor > Saldo)
            {
                throw new InvalidOperationException("Saldo insuficiente");
            }

            Saldo -= valor;

            Historico.Add(new Transacao("Saída de R$" + valor, valor, DateTime.Now));
        }

        public void Transferir(ContaMovimento contaDestino, decimal valor)
        {
            if (valor > Saldo)
                throw new InvalidOperationException("Saldo insuficiente");

            Saldo -= valor;

            contaDestino.Saldo += valor;

            Historico.Add(new Transacao(
                $"Transferência de R${valor} enviada para {contaDestino.NomeContaMovimento}.",
                valor,
                DateTime.Now
            ));

            contaDestino.Historico.Add(new Transacao(
                $"Transferência de R${valor} recebida de {NomeContaMovimento}", 
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
