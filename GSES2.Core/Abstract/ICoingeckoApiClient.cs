using GSES2.Domain.Transport.Responses;

namespace GSES2.Core.Abstract;
public interface ICoingeckoApiClient
{
    Task<GetBtcToUahRateResponse> GetBtcToUahRateAsync(CancellationToken cancellationToken = default);
}
