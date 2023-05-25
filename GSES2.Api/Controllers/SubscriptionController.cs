using GSES2.Domain.Exceptions;
using GSES2.Domain.Transport.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace GSES2.Api.Controllers;
public class SubscriptionController : Controller
{
    private readonly IMediator _mediator;

    public SubscriptionController(IMediator mediator) => _mediator = mediator;
    
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

    [HttpPost("sendEmails")]
    public async Task<IActionResult> SendEmails()
    {
        await _mediator.Send(new SendEmailsRequest());
        return Ok();
    }
}
