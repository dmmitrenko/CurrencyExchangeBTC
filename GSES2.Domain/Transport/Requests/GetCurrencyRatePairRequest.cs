using GSES2.Domain.Enums;
using MediatR;

namespace GSES2.Domain.Transport.Requests;
public class GetCurrencyRatePairRequest : IRequest<decimal> 
{
    public Coin Coin { get; set; }
    public Currency Currency { get; set; }
}
