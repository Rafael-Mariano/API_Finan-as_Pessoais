namespace Finance.Api.Domain
{
    public class Transacao
    {
        public string Tipo { get; }
        public decimal Valor { get; }
        public DateTime Data { get; }

        public Transacao(string tipo, decimal valor, DateTime data)
        {
            Tipo = tipo;
            Valor = valor;
            Data = DateTime.Now;
        }
    }
}
