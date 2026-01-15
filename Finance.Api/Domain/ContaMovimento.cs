using System.Text.Encodings.Web;

namespace Finance.Api.Domain
{
    public class ContaMovimento
    {
        public int Id { get; set; }
        public string NomeContaMovimento { get; set; }
        public decimal Saldo { get; set; }
        public List<Transacao> Historico { get; set; } = new List<Transacao>();

        public ContaMovimento() { }

        public ContaMovimento(string nomeConta)
        {
            NomeContaMovimento = nomeConta;
            Saldo = 0m;
        }

        public void Entrada(decimal valor)
        {
            Saldo += valor;

            Historico.Add(new Transacao("Entrada", valor, DateTime.Now));
        }

        public void Saida(decimal valor)
        {
            if (valor > Saldo)
            {
                throw new InvalidOperationException("Saldo insuficiente");
            }

            Saldo -= valor;

            Historico.Add(new Transacao("Saída", valor, DateTime.Now));
        }

        public void Transferir(ContaMovimento contaDestino, decimal valor)
        {
            if (valor > Saldo)
                throw new InvalidOperationException("Saldo insuficiente");

            Saldo -= valor;

            contaDestino.Saldo += valor;

            Historico.Add(new Transacao(
                "Transferência enviada",
                valor,
                DateTime.Now
            ));

            contaDestino.Historico.Add(new Transacao(
                "Transferência recebida", 
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
