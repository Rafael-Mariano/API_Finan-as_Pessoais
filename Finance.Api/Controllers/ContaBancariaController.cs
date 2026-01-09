using Finance.Api.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Finance.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaBancariaController : ControllerBase
    {
        private static readonly Dictionary<int, ContaBancaria> _contas = new();
        private static int _id = 1;

        [HttpPost]
        public IActionResult CriarConta(string titular)
        {
            var conta = new ContaBancaria(titular);
            _contas[_id] = conta;
            return Ok(new { Id = _id++, conta.Titular, conta.Saldo });
        }

        [HttpPost("{id}/depositar")]
        public IActionResult Depositar(int id, decimal valor)
        {
            _contas[id].Depositar(valor);
            return Ok(_contas[id].Saldo);
        }

        [HttpPost("{id}/sacar")]
        public IActionResult Sacar(int id, decimal valor)
        {
            _contas[id].Sacar(valor);
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
            var extrato = _contas[id].Saldo;
            return Ok(extrato);
        }
    }
}
