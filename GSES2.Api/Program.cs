using GSES2.Api;
using GSES2.Api.Extensions;
using GSES2.Application.RequestHandlers;
using MediatR;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureApiClients();
builder.Services.ConfigureOptions(builder.Configuration);
builder.Services.ConfigureServices(builder.Configuration);

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

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
