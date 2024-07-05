using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Repository;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovimentoController : ControllerBase
    {        
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MovimentoController(IMovimentoRepository movimentoRepository, IContaCorrenteRepository contaCorrenteRepository, IIdempotenciaRepository idempotenciaRepository, IUnitOfWork unitOfWork)
        {
            _movimentoRepository = movimentoRepository;
            _contaCorrenteRepository = contaCorrenteRepository;
            _idempotenciaRepository = idempotenciaRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("movimentar")]
        public async Task<IActionResult> MovimentarConta([FromBody] MovimentoRequest request, [FromHeader] string? chaveIdempotencia = null)
        {
            if (await _idempotenciaRepository.VerificarChaveExistenteAsync(chaveIdempotencia))
            {
                return Conflict("Requisição já processada.");
            }

            try
            {
                var conta = await _contaCorrenteRepository.GetContaCorrenteById(request.IdContaCorrente);
                if (conta == null) return BadRequest("INVALID_ACCOUNT");
                if (!conta.Ativo) return BadRequest("INACTIVE_ACCOUNT");
                if (request.Valor <= 0) return BadRequest("INVALID_VALUE");
                if (request.TipoMovimento != 'C' && request.TipoMovimento != 'D') return BadRequest("INVALID_TYPE");

                var movimento = new Movimento(Guid.NewGuid().ToString(), request.IdContaCorrente, DateTime.UtcNow, request.TipoMovimento, request.Valor, conta);

                await _movimentoRepository.AddMovimento(movimento);
                await _unitOfWork.CommitAsync();

                return Ok(new { IdMovimento = movimento.IdMovimento.ToUpper() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
            
        }
    }
}