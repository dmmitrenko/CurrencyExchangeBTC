using GSES2.Domain.Transport.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GSES2.Api.Controllers;

public class RateController : Controller
{
    private readonly IMediator _mediator;

    public RateController(IMediator mediator) => _mediator = mediator;

    [HttpGet("rate")]
    public async Task<IActionResult> GetBtcToUahResponse(CancellationToken cancellationToken = default)
    {
        var rate = await _mediator.Send(new GetCurrencyRatePairRequest(), cancellationToken);
        return Ok(rate);
    }
        
}
