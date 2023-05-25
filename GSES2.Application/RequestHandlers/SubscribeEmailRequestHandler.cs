using GSES2.Domain.Exceptions;
using GSES2.Domain.Transport.Requests;
using GSES2.Repository;
using MediatR;
using System.Net;

namespace GSES2.Application.RequestHandlers;
public class SubscribeEmailRequestHandler : IRequestHandler<SubscribeEmailRequest>
{
    private readonly IRepository _repository;

    public SubscribeEmailRequestHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(SubscribeEmailRequest request, CancellationToken cancellationToken)
    {
        if (await _repository.IsEmailExists(request.Email))
        {
            throw new DomainException("Email already subscribed", (int)HttpStatusCode.Conflict);
        }

        await _repository.SubscribeEmailAsync(request.Email);
    }
}
