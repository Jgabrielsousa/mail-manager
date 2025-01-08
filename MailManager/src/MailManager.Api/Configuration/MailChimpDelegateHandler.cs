using System.Text;

namespace mail_manager.Configuration
{
    public class MailChimpDelegateHandler : DelegatingHandler
    {
       

        public MailChimpDelegateHandler()
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string apiKey = "5a40a0b09761b0fca355a0482eda3eea-us16";
            var byteArray = Encoding.ASCII.GetBytes($"anystring:{apiKey}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
