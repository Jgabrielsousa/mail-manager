using MailManager.Application.Services.MockContacts.Responses;
using Refit;

namespace MailManager.Application.Services.MockContacts;

public interface IMockContactsApi {

    [Get("/contacts")]
    Task<IEnumerable<ContactsResponse>> Get();

}
