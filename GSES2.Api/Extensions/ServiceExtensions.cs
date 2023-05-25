using GSES2.Application.RestClients;
using GSES2.Application.Settings;
using GSES2.Core.Abstract;
using GSES2.Repository;
using Microsoft.Extensions.Options;
using SendGrid.Extensions.DependencyInjection;

namespace GSES2.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureApiClients(this IServiceCollection services)
    {
        services.AddHttpClient<ICoingeckoApiClient, CoingeckoApiClient>((provider, client) =>
        {
            client.BaseAddress = new Uri(
                provider.GetService<IOptions<CoingeckoApiSettings>>()?.Value.ApiBaseUrl ??
                throw new ArgumentNullException(nameof(CoingeckoApiSettings.ApiBaseUrl)));
        });
    }

    public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CoingeckoApiSettings>(configuration.GetSection("CoingeckoApiOptions"));
        services.Configure<SendGridApiSettings>(configuration.GetSection("SendGridApiSettings"));
    }

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSendGrid(options =>
            options.ApiKey = configuration["SendGridApiSettings:ApiKey"]);

        services.AddScoped<IRepository, GSES2.Repository.Repository>();
    }
}
