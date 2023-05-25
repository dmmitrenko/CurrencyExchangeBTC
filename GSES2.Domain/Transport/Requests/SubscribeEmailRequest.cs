using GSES2.Domain.Transport.Responses;
using MediatR;

namespace GSES2.Domain.Transport.Requests;
public class SubscribeEmailRequest : ApiRequestBase, IRequest
{
    public const string Route = "subscribe";

    public string Email { get; set; }
}
