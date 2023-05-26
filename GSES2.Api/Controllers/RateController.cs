using GSES2.Domain.Transport.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GSES2.Api.Controllers;

public class RateController : Controller
{
    private readonly IMediator _mediator;

    public RateController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Отримати поточний курс BTC до UAH
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("rate")]
    public async Task<IActionResult> GetBtcToUahResponse(CancellationToken cancellationToken = default)
    {
        var rate = await _mediator.Send(new GetCurrencyRatePairRequest(), cancellationToken);
        return Ok(rate);
    }
        
}
