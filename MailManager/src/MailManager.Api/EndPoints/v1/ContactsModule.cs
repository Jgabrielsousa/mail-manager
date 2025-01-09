
using MailManager.Application.Dtos;
using MailManager.Application.Services.MailChimp;
using MailManager.Application.Services.MockContacts;
using MailManager.Application.UseCases.Base;
using MailManager.Application.UseCases.SyncContacts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
        }

        private static async Task<IResult> SyncData(
        [FromServices] IUseCaseHandler<SyncContactsCommand,SyncContactsHandler> _syncCommand,
        CancellationToken cancellationToken)
        {
            var result = _syncCommand.Handle(null, cancellationToken);
            return Results.Ok(result.Result.Data);
        }
    }
}
