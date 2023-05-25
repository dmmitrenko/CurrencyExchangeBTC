using GSES2.Application.RequestHandlers;
using GSES2.Application.RestClients;
using GSES2.Application.Settings;
using GSES2.Core.Abstract;
using GSES2.Repository;
using MediatR;
using Microsoft.Extensions.Options;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CoingeckoApiSettings>(builder.Configuration.GetSection("CoingeckoApiOptions"));
builder.Services.Configure<SendGridApiSettings>(builder.Configuration.GetSection("SendGridApiSettings"));
builder.Services.AddSendGrid(options => 
    options.ApiKey = builder.Configuration["SendGridApiSettings:ApiKey"]);
builder.Services.AddHttpClient<ICoingeckoApiClient, CoingeckoApiClient>((provider, client) =>
{
    client.BaseAddress = new Uri(
        provider.GetService<IOptions<CoingeckoApiSettings>>()?.Value.ApiBaseUrl ??
        throw new ArgumentNullException(nameof(CoingeckoApiSettings.ApiBaseUrl)));
});
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(GetCurrencyRatePairRequestHandler).Assembly);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
