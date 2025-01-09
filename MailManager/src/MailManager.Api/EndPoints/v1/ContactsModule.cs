
using MailManager.Api.Configuration;
using MailManager.Application.Abstraction.Config;
using MailManager.Application.Dtos;
using MailManager.Application.UseCases.Base;
using MailManager.Application.UseCases.SyncContacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace mail_manager.EndPoints.v1
{
    public class ContactsModule : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("contacts")
           .WithTags("Contacts");

            group.MapGet("sync", SyncData)
            .WithOpenApi()
            .Produces<SyncedContactsDto>(StatusCodes.Status200OK);


            group.MapGet("config", Config)
            .WithOpenApi()
            .Produces<AppSettings>(StatusCodes.Status200OK);


        }

        private static async Task<IResult> SyncData(
        [FromServices] IUseCaseHandler<SyncContactsCommand,SyncContactsHandler> _syncCommand,
        CancellationToken cancellationToken)
        {
            var result = _syncCommand.Handle(null, cancellationToken);
            return Results.Ok(result.Result.Data);
        }

        private static async Task<IResult> Config(IOptions<AppSettings> appSettings, CancellationToken cancellationToken)
        {
            var settings = appSettings.Value;
            AppSettings aux = new AppSettings();
            aux.Sources = new List<Sources>();

            foreach (var item in settings.Sources)
            {
                aux.Sources.Add(new Sources() { 
                    ApiKey = item.ApiKey.ToHide(), 
                    ApiId= item.ApiId.ToHide(), 
                    Name = item.Name,
                    Url = item.Url.ToHide(), 
                });
            }
            return Results.Ok(aux);
        }
    }
}
