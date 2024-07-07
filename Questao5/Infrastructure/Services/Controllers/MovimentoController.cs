using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Questao5.Application.ContaCorrente.Queries;
using Questao5.Application.Movimento.Commands;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Repository;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class MovimentoController : ControllerBase
    {
        private readonly IIdempotenciaRepository _idempotenciaRepository;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public MovimentoController(IIdempotenciaRepository idempotenciaRepository, IMediator mediator, IUnitOfWork unitOfWork)
        {
            _idempotenciaRepository = idempotenciaRepository;
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Consulta o movimento da conta
        /// </summary>
        /// <param name="request"></param>
        /// <param name="chaveIdempotencia"></param>
        /// <returns>Retorna o identificador do movimento: IdMovimento</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Post /MovimentarConta
        ///     {
        ///         "idContaCorrente":"B6BAFC09 -6967-ED11-A567-055DFA4A16C9"
        ///         "tipoMovimento": "C",
        ///         "valor": 150.43
        ///     }
        /// </remarks>
        /// <response code="200">Retorna o movimento da conta</response>
        /// <response code="400">INVALID_ACCOUNT - conta corrente nula ou conta corrente desativada</response>
        /// <response code="400">INVALID_VALUE - valor do movimento menor ou igual a zero</response>
        /// <response code="400">INVALID_TYPE - tipo do movimento diferente de C ou D (crédito/débito)</response>
        /// <response code="500">Erro no servidor</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("movimentar")]
        public async Task<IActionResult> MovimentarConta([FromBody] AddMovimentoCommand request, [FromHeader] string? chaveIdempotencia = null)
        {
            if (string.IsNullOrEmpty(chaveIdempotencia))
            {
                return BadRequest("Chave de idempotência é obrigatória.");
            }

            if (await _idempotenciaRepository.VerificarChaveExistenteAsync(chaveIdempotencia))
            {
                var resultadoExistente = await _idempotenciaRepository.ObterResultadoPorChaveAsync(chaveIdempotencia);
                return Conflict($"Requisição já processada. Resultado: {resultadoExistente}");
            }

            try
            {
                await _idempotenciaRepository.RegistrarChaveAsync(chaveIdempotencia, JsonConvert.SerializeObject(request));

                // var conta = await _contaCorrenteRepository.GetContaCorrenteById(new GetContaCorrenteByIdRequest { IdContaCorrente = request.IdContaCorrente });
                var queryContaCorrente = new GetContasCorrenteByIdQuery(request.IdContaCorrente);
                var contaCorrente = await _mediator.Send(queryContaCorrente);

                if (contaCorrente == null) return BadRequest("INVALID_ACCOUNT");
                if (!contaCorrente.Ativo) return BadRequest("INACTIVE_ACCOUNT");
                if (request.Valor <= 0) return BadRequest("INVALID_VALUE");
                if (request.TipoMovimento != 'C' && request.TipoMovimento != 'D') return BadRequest("INVALID_TYPE");                

                var movimento = await _mediator.Send(request);
                await _idempotenciaRepository.AtualizarResultadoAsync(chaveIdempotencia, JsonConvert.SerializeObject(movimento.IdMovimento));
                await _unitOfWork.CommitAsync();

                return Ok(new { IdMovimento = movimento });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }

        }
    }
}