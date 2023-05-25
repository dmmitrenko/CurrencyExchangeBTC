using GSES2.Core.Abstract;
using GSES2.Domain.Transport.Requests;
using MediatR;

namespace GSES2.Application.RequestHandlers;
public class GetCurrencyRatePairRequestHandler : IRequestHandler<GetCurrencyRatePairRequest, decimal>
{
    private readonly ICoingeckoApiClient _coinGeckoApiClient;

    public GetCurrencyRatePairRequestHandler(ICoingeckoApiClient coinGeckoApiClient)
    {
        _coinGeckoApiClient = coinGeckoApiClient;
    }

    public async Task<decimal> Handle(GetCurrencyRatePairRequest request, CancellationToken cancellationToken)
    {
        var response = await _coinGeckoApiClient.GetBtcToUahRateAsync(cancellationToken);

        return response.Bitcoin.Uah;
    }
}
