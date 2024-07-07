using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.ContaCorrente.Queries;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;

        private readonly IMediator _mediator;

        public ContaCorrenteController(IContaCorrenteRepository contaCorrenteRepository, IMovimentoRepository movimentoRepository, IMediator mediator)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentoRepository = movimentoRepository;
            _mediator = mediator;
        }

        /// <summary>
        /// Consulta o saldo da conta corrente
        /// </summary>
        /// <param name="idContaCorrente"></param>
        /// <returns>Retorna a consulta do saldo</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Get /ConsultarSaldo
        ///     idContaCorrente=B6BAFC09 -6967-ED11-A567-055DFA4A16C9
        /// </remarks>
        /// <response code="200">Retorna o saldo da conta corrente</response>
        /// <response code="400">INVALID_ACCOUNT - conta corrente nula ou conta corrente desativada</response>
        [HttpGet("saldo/{idContaCorrente}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ConsultarSaldo(string idContaCorrente)
        {
            // Consulta para obter os detalhes da conta corrente
            // var contaCorrente = await _contaCorrenteRepository.GetContaCorrenteById(new GetContaCorrenteByIdRequest { IdContaCorrente = idContaCorrente});
            var queryContaCorrente = new GetContasCorrenteByIdQuery(idContaCorrente);
            var contaCorrente = await _mediator.Send(queryContaCorrente);

            if (contaCorrente == null) return BadRequest("INVALID_ACCOUNT");
            if (!contaCorrente.Ativo) return BadRequest("INACTIVE_ACCOUNT");

            // Consulta para obter a soma dos créditos
            // var movimentos = await _movimentoRepository.GetMovimentoByIdContaCorrente(idContaCorrente);
            var queryMovimento = new GetMovimentoByIdQuery(idContaCorrente);
            var movimentos = await _mediator.Send(queryMovimento);

            var creditos = movimentos.Where(m => m.TipoMovimento == 'C').Sum(m => m.Valor);
            var debitos = movimentos.Where(m => m.TipoMovimento == 'D').Sum(m => m.Valor);

            var saldo = creditos - debitos;

            return Ok(new
            {
                Numero = contaCorrente.Numero,
                Nome = contaCorrente.Nome,
                DataHoraConsulta = DateTime.UtcNow,
                SaldoAtual = Math.Round(saldo,2)
            });
        }
    }
}