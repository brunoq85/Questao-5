using Microsoft.AspNetCore.Mvc;
using Questao5.Domain.Interfaces;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;

        public ContaCorrenteController(IContaCorrenteRepository contaCorrenteRepository, IMovimentoRepository movimentoRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentoRepository = movimentoRepository;
        }

        [HttpGet("saldo/{idContaCorrente}")]
        public async Task<IActionResult> ConsultarSaldo(string idContaCorrente)
        {
            // Consulta para obter os detalhes da conta corrente
            var contaCorrente = await _contaCorrenteRepository.GetContaCorrenteById(idContaCorrente);

            if (contaCorrente == null) return BadRequest("INVALID_ACCOUNT");
            if (!contaCorrente.Ativo) return BadRequest("INACTIVE_ACCOUNT");

            // Consulta para obter a soma dos créditos
            var movimentos = await _movimentoRepository.GetMovimentoByIdContaCorrente(idContaCorrente);

            var creditos = movimentos.Where(m => m.TipoMovimento == 'C').Sum(m => m.Valor);
            var debitos = movimentos.Where(m => m.TipoMovimento == 'D').Sum(m => m.Valor);

            var saldo = creditos - debitos;

            return Ok(new
            {
                Numero = contaCorrente.Numero,
                Nome = contaCorrente.Nome,
                DataHoraConsulta = DateTime.UtcNow,
                SaldoAtual = saldo
            });
        }
    }
}