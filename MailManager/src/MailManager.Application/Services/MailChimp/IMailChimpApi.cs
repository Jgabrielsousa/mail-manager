using MailManager.Application.Services.MailChimp.Reponses;
using MailManager.Application.Services.MailChimp.Requests;
using Refit;

namespace MailManager.Application.Services.MailChimp;

public interface IMailChimpApi
{
    [Post("/lists/{listId}/members")]
    Task<ContactResponse> Post(string listId, [Body]ContactRequest request);
}
