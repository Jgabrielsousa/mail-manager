using MailManager.Application.Abstraction;
using MailManager.Application.Abstraction.Config;
using MailManager.Application.Dtos;
using MailManager.Application.Services.MailChimp;
using MailManager.Application.Services.MockContacts;
using MailManager.Application.UseCases.Base;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MailManager.Application.UseCases.SyncContacts;

public class SyncContactsHandler : HandlerBase<SyncContactsCommand, SyncContactsHandler>
{
    private readonly IMailChimpApi _mailChimpApi;
    private readonly IMockContactsApi _mockContactsApi;
    private readonly AppSettings _appSettings;
    private readonly ILogger<SyncContactsHandler> _logger;
    public SyncContactsHandler(IMailChimpApi mailChimpApi, IMockContactsApi mockContactsApi, IOptions<AppSettings> appSettings,ILogger<SyncContactsHandler> logger)
    {
        _mailChimpApi = mailChimpApi;
        _mockContactsApi = mockContactsApi;
        _appSettings = appSettings.Value;
        _logger= logger;
    }
    public override async Task<Result> Execute(SyncContactsCommand? command, CancellationToken cancellationToken = default)
    {
        var usersMock = await _mockContactsApi.Get();
        var usersSynced = new List<Contact>();

        _logger.LogInformation("usersMock | {Count}", usersMock.Count());
        

        foreach (var user in usersMock.Take(1)) {
            try
            {
                await _mailChimpApi.Post(GetListId(), user);
                usersSynced.Add(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Handler} | Exception {Message}", nameof(SyncContactsHandler), e.Message);
            }
        }

        var result = new SyncedContactsDto(usersSynced.Count, usersSynced);

        return new Result(result,[]);
    }
    private string GetListId()
       => _appSettings.Sources.First(c => c.Name == StringConst.MailChimpDefinition).ApiKey;
}