using Refit;
using System.Text.Json.Serialization;

namespace MailManager.Application.Services.MailChimp.Requests;

public class ContactRequest
{
    [JsonPropertyName("email_address")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("merge_fields")]
    public MergeFields MergeFields { get; set; }
}

public class MergeFields
{
    [JsonPropertyName("FNAME")]
    public string FirstName { get; set; }

    [JsonPropertyName("LNAME")]
    public string LastName { get; set; }
}


