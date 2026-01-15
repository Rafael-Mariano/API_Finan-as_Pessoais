using Finance.Api.Data;
using Finance.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Finance.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaMovimentoController : ControllerBase
    {
        private readonly DbContext_ _context;
        public ContaMovimentoController(DbContext_ context) 
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CriarConta(string nomeConta)
        {
            // Criar conta
            var conta = new ContaMovimento(nomeConta);

            // Sinaliza a entrada de uma nova conta
            _context.contaMovimentoTbl.Add(conta);

            // Salva as informações no banco de dados
            await _context.SaveChangesAsync();

            return Ok(new { Id = conta.Id, conta.NomeContaMovimento, conta.Saldo });
        }

        [HttpPost("{id}/entrada")]
        public async Task<IActionResult> Entrada(int id, decimal valor)
        {
            // Buscar informação do Id
            var conta = await _context.contaMovimentoTbl
                .FirstOrDefaultAsync(conta => conta.Id == id);

            // Registrar a entrada do valor
            conta.Entrada(valor);

            // Salvar no banco de dados
            await _context.SaveChangesAsync();

            return Ok($"Entrada de R${valor.ToString("F2")} na conta {conta.NomeContaMovimento}.");
        }

        [HttpPost("{id}/saida")]
        public async Task<IActionResult> Saida(int id, decimal valor)
        {
            var conta = await _context.contaMovimentoTbl
                .FirstOrDefaultAsync(conta => conta.Id == id);

            conta.Saida(valor);

            await _context.SaveChangesAsync();

            return Ok($"Saída de R${valor.ToString("F2")} da conta {conta.NomeContaMovimento}.");
        }

        [HttpPost("{id}/transferir/{destinoId}")]
        public async Task<IActionResult> Transferir(int id, int destinoId, decimal valor)
        {
            var origem = await _context.contaMovimentoTbl
                .FirstOrDefaultAsync(conta => conta.Id == id);

            var destino = await _context.contaMovimentoTbl
                .FirstOrDefaultAsync(conta => conta.Id == destinoId);

            origem.Transferir(destino, valor);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Origem = new
                {
                    SaldoOrigem = origem.Saldo,
                    UltimoHistorico = origem.Historico.Last()
                },
                Destino = new
                {
                    SaldoDestino = destino.Saldo,
                    UltimoHistorico = destino.Historico.Last()
                }
            });
        }

        [HttpGet("{id}/saldo")]
        public async Task<IActionResult> Saldo(int id)
        {

            var conta = await _context.contaMovimentoTbl
                .FirstOrDefaultAsync(conta => conta.Id == id);

            return Ok($"O sado da conta {conta.Id} é de R${conta.Saldo}.");
        }
    }
}
