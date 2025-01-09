using MailManager.Application.Abstraction;
using MailManager.Application.Abstraction.Config;
using Microsoft.Extensions.Options;
using System.Text;

namespace mail_manager.Configuration
{
    public class MailChimpDelegateHandler : DelegatingHandler
    {
        private readonly AppSettings _appSettings;
        public MailChimpDelegateHandler(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string apiKey = _appSettings.Sources.First(c => c.Name == StringConst.MailChimpDefinition).ApiKey;
            var byteArray = Encoding.ASCII.GetBytes($"anystring:{apiKey}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
