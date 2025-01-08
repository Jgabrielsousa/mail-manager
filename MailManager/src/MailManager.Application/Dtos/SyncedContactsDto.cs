namespace MailManager.Application.Dtos
{
    public record SyncedContactsDto(int SyncedContacts,List<Contact> Contacts);
    public record Contact(string FirstName,string LastName,string Email);
}
