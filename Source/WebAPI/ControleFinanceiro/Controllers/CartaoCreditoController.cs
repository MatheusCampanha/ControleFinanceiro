using ControleFinanceiro.Domain.CartoesCredito.Commands;
using ControleFinanceiro.Domain.CartoesCredito.Interfaces.Handlers;
using ControleFinanceiro.Domain.CartoesCredito.Interfaces.Repositories;
using ControleFinanceiro.Domain.CartoesCredito.Queries;
using ControleFinanceiro.Domain.CartoesCredito.Queries.Results;
using ControleFinanceiro.Domain.CartoesCredito.Results;
using ControleFinanceiro.Domain.Core.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ControleFinanceiro.Controllers
{
    [Route("")]
    [ApiController]
    public class CartaoCreditoController : BaseController
    {
        private readonly ICartaoCreditoHandler _handler;
        private readonly ICartaoCreditoRepository _repository;

        public CartaoCreditoController(ICartaoCreditoHandler handler, ICartaoCreditoRepository repository)
        {
            _handler = handler;
            _repository = repository;
        }

        [HttpGet]
        [Route("v1/cartoesCredito/{cartaoCreditoId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CartaoCreditoQueryResult))]
        public async Task<IActionResult> Get(int cartaoCreditoId)
        {
            return ResultFirst(await _repository.ObterResult(cartaoCreditoId));
        }

        [HttpGet]
        [Route("v1/cartoesCredito")]
        [ProducesResponseType((int)HttpStatusCode.PartialContent, Type = typeof(QueryResult<CartaoCreditoQueryResult>))]
        public IActionResult GetPaginado([FromQuery] ConsultarCartaoCreditoQuery query)
        {
            return ResultFirst(_repository.ObterResult(query));
        }

        [HttpPost]
        [Route("v1/cartoesCredito")]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(IncluirCartaoCreditoCommandResult))]
        public async Task<IActionResult> Post(IncluirCartaoCreditoCommand command)
        {
            var result = await _handler.Handle(command);
            return ResultFirst(result);
        }

        [HttpPut]
        [Route("v1/cartoesCredito/{cartaoCreditoId}")]
        public async Task<IActionResult> Alterar(int cartaoCreditoId, [FromBody] AlterarCartaoCreditoCommand command)
        {
            command.DefinirCartaoCreditoId(cartaoCreditoId);
            var result = await _handler.Handle(command);
            return Result(result);
        }

        [HttpDelete]
        [Route("v1/cartoesCredito/{cartaoCreditoId}")]
        public async Task<IActionResult> Remover(int cartaoCreditoId)
        {
            var result = await _handler.Handle(cartaoCreditoId);
            return Result(result);
        }
    }
}