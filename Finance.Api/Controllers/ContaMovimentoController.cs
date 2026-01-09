using Finance.Api.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Finance.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaMovimentoController : ControllerBase
    {
        private static readonly Dictionary<int, ContaMovimento> _contas = new();
        private static int _id = 1;

        [HttpPost]
        public IActionResult CriarConta(string nomeConta)
        {
            var conta = new ContaMovimento(nomeConta);
            _contas[_id] = conta;
            return Ok(new { Id = _id++, conta.NomeContaMovimento, conta.Saldo });
        }

        [HttpPost("{id}/entrada")]
        public IActionResult Entrada(int id, decimal valor)
        {
            _contas[id].Entrada(valor);
            return Ok(_contas[id].Saldo);
        }

        [HttpPost("{id}/saida")]
        public IActionResult Sacar(int id, decimal valor)
        {
            _contas[id].Saida(valor);
            return Ok(_contas[id].Saldo);
        }

        [HttpPost("{id}/transferir/{destinoId}")]
        public IActionResult Transferir(int id, int destinoId, decimal valor)
        {
            _contas[id].Transferir(_contas[destinoId], valor);
            return Ok(new { SaldoOrigem = _contas[id].Saldo, SaldoDestino = _contas[destinoId] });
        }

        [HttpGet("{id}/saldo")]
        public IActionResult Saldo(int id)
        {
            var saldo = _contas[id].Saldo;
            return Ok(saldo);
        }
    }
}
