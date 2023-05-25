using GSES2.Domain.Transport.Requests;
using MediatR;
using SendGrid.Helpers.Mail;
using SendGrid;
using GSES2.Repository;
using Microsoft.Extensions.Options;
using GSES2.Application.Settings;
using GSES2.Domain.Exceptions;
using GSES2.Core.Abstract;

namespace GSES2.Application.RequestHandlers;
public class SendEmailsRequestHandler : IRequestHandler<SendEmailsRequest, Unit>
{
    private readonly ISendGridClient _sendGridClient;
    private readonly ICoingeckoApiClient _coingeckoApiClient;
    private readonly IRepository _repository;
    private readonly SendGridApiSettings _sendGridSettings;

    public SendEmailsRequestHandler(
        ISendGridClient sendGridClient,
        ICoingeckoApiClient coingeckoApiClient,
        IRepository repository,
        IOptions<SendGridApiSettings> sendGridSettings)
    {
        _sendGridClient = sendGridClient;
        _coingeckoApiClient = coingeckoApiClient;
        _repository = repository;
        _sendGridSettings = sendGridSettings.Value;
    }

    public async Task<Unit> Handle(SendEmailsRequest request, CancellationToken cancellationToken)
    {
        var senderEmail = _sendGridSettings.SenderEmail;
        var senderName = _sendGridSettings.SenderName;
        var subject = "Bitcoin/UAH currency rate";
        var body = await _coingeckoApiClient.GetBtcToUahRateAsync();

        var msg = new SendGridMessage()
        {
            From = new EmailAddress(senderEmail, senderName),
            Subject = subject,
            PlainTextContent = $"The current rate is 1 BTC = {body.Bitcoin.Uah} UAH"
        };

        var emails = await _repository.GetSubscribedEmailsAsync();
        foreach (var email in emails)
        {
            msg.AddTo(new EmailAddress(email));
        }

        var response = await _sendGridClient.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            throw new DomainException("Something went wrong.", (int)response.StatusCode);
        }

        return Unit.Value;
    }
}
