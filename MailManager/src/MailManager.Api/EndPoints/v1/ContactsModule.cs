
using MailManager.Application.Dtos;
using MailManager.Application.Services.MailChimp;
using MailManager.Application.Services.MockContacts;
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

        private static async Task<IResult> SyncData(CancellationToken cancellationToken)
        {
            return Results.Ok();
        }
    }
}
