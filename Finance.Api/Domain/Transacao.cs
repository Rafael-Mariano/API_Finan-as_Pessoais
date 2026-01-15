using System.Text.Json.Serialization;

namespace Finance.Api.Domain
{
    public class Transacao
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public int ContaMovimentoId { get; set; }
        [JsonIgnore]
        public ContaMovimento contaMovimento { get; set; }

        public Transacao() { }

        public Transacao(string tipo, decimal valor, DateTime data)
        {
            Tipo = tipo;
            Valor = valor;
            Data = DateTime.Now;
        }
    }
}
