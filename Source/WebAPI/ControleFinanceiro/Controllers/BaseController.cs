using ControleFinanceiro.Domain.Core.Commands;
using ControleFinanceiro.Domain.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ControleFinanceiro.Controllers
{
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(CommandResult))]
    [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(CommandResult))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(CommandResult))]
    [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity, Type = typeof(CommandResult))]
    public class BaseController : ControllerBase
    {
        [NonAction]
        public IActionResult Result(CommandResult result)
        {
            var statusCode = result.StatusCode != 412 ? result.StatusCode : 422;

            if (statusCode == HttpStatusCode.NoContent.GetHashCode())
                return StatusCode(204);

            return StatusCode(statusCode, result);
        }

        [NonAction]
        public IActionResult Result(int statusCode, object result)
        {
            statusCode = statusCode != 412 ? statusCode : 422;

            if (statusCode == HttpStatusCode.NoContent.GetHashCode())
                return StatusCode(204);

            return StatusCode(statusCode, result);
        }

        [NonAction]
        public IActionResult Result<T>(Result<T> result) where T : class
        {
            var statusCode = result.StatusCode != 412 ? result.StatusCode : 422;

            if (statusCode == HttpStatusCode.NoContent.GetHashCode())
                return StatusCode(204);

            if (result != null && result.QueryResult != null && result.QueryResult.Registros != null && result.QueryResult.Registros.Count > 0)
                return StatusCode(statusCode, result.QueryResult);

            var errorResult = new CommandResult(result!.StatusCode, result);
            return StatusCode(errorResult.StatusCode, errorResult);
        }

        [NonAction]
        public IActionResult ResultFirst<T>(Result<T> result) where T : class
        {
            var statusCode = result.StatusCode != 412 ? result.StatusCode : 422;

            if (statusCode == HttpStatusCode.NoContent.GetHashCode())
                return StatusCode(204);

            if (result != null && result.QueryResult != null && result.QueryResult.Registros != null && result.QueryResult.Registros.Count > 0)
                return StatusCode(statusCode, result.QueryResult.Registros.FirstOrDefault());

            var errorResult = new CommandResult(result!.StatusCode, result);
            return StatusCode(errorResult.StatusCode, errorResult);
        }
    }
}