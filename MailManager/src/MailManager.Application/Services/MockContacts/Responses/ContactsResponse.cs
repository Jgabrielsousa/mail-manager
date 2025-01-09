namespace MailManager.Application.Services.MockContacts.Responses;
using Chimp = Services.MailChimp.Requests;
using DtoView = MailManager.Application.Dtos;

public class ContactsResponse
{
    public DateTime CreatedAt { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
    public string Id { get; set; }

    public static implicit operator Chimp.ContactRequest(ContactsResponse response) =>
      new Chimp.ContactRequest() { 
          Status= "subscribed",
          EmailAddress = response.Email,
          MergeFields = new Chimp.MergeFields() { 
          FirstName = response.FirstName,
          LastName = response.LastName
          } 
      };

    public static implicit operator DtoView.Contact(ContactsResponse response) =>
      new DtoView.Contact(response.FirstName,response.LastName,response.Email);

}
