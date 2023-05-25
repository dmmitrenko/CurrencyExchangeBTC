using GSES2.Application.Settings;
using GSES2.Core.Abstract;
using GSES2.Domain.Enums;
using GSES2.Domain.Transport.Responses;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;

namespace GSES2.Application.RestClients;
public class CoingeckoApiClient : ApiClientBase, ICoingeckoApiClient
{
    private readonly CoingeckoApiSettings _coinApiSettings;

    public CoingeckoApiClient(
        HttpClient httpClient,
        IOptions<CoingeckoApiSettings> coinApiSettings) 
        : base(httpClient)
    {
        _coinApiSettings = coinApiSettings.Value;
    }

    public async Task<GetBtcToUahRateResponse> GetBtcToUahRateAsync(CancellationToken cancellationToken = default)
    {
        var id = Coin.Bitcoin.ToString();
        var currency = Currency.UAH.ToString();

        var parameters = new Dictionary<string, string>
        {
            {CoingeckoApiParameters.Ids, id },
            {CoingeckoApiParameters.VsCurrencies, currency }
        };

        var uri = BuildRequestUrl(_coinApiSettings.GetCurrencyConversionEndpoint, parameters);
        return await GetAsync<GetBtcToUahRateResponse>(uri, cancellationToken);
    }
}
