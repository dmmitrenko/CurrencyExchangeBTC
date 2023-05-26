using GSES2.Domain.Exceptions;
using GSES2.Domain.Transport.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace GSES2.Api.Controllers;

public class SubscriptionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubscriptionController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Підписати емейл на отримання поточного курсу
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost("subscribe")]
    public async Task<IActionResult> SubscribeEmail([FromQuery] [EmailAddress] string email)
    {
        if (!ModelState.IsValid)
        {
            throw new DomainException("Email incorrect!", (int)HttpStatusCode.BadRequest);
        }

        await _mediator.Send(new SubscribeEmailRequest { Email = email });
        return CreatedAtAction(nameof(SubscribeEmail), email, null);
    }

    /// <summary>
    /// Відправити e-mail з поточним курсом на всі підписані електронні пошти.
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("sendEmails")]
    public async Task<IActionResult> SendEmails()
    {
        await _mediator.Send(new SendEmailsRequest());
        return Ok();
    }
}
