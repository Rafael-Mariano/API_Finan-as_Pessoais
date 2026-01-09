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
            return Ok($"Entrada de R${_contas[id].Saldo} na conta {_contas[id].NomeContaMovimento}.");
        }

        [HttpPost("{id}/saida")]
        public IActionResult Saida(int id, decimal valor)
        {
            _contas[id].Saida(valor);
            return Ok($"Saída de R${valor} da conta {_contas[id].NomeContaMovimento}.");
        }

        [HttpPost("{id}/transferir/{destinoId}")]
        public IActionResult Transferir(int id, int destinoId, decimal valor)
        {
            _contas[id].Transferir(_contas[destinoId], valor);
            return Ok(new
            {
                Origem = new
                {
                    SaldoOrigem = _contas[id].Saldo,
                    UltimoHistorico = _contas[id].Historico.Last()
                },
                Destino = new
                {
                    SaldoDestino = _contas[destinoId].Saldo,
                    UltimoHistorico = _contas[id].Historico.Last()
                }
            });
        }

        [HttpGet("{id}/saldo")]
        public IActionResult Saldo(int id)
        {
            var saldo = _contas[id].Saldo;
            return Ok($"O sado da conta {_contas[id].NomeContaMovimento} é de R${saldo}.");
        }
    }
}
