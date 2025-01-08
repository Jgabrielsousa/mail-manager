using Asp.Versioning;
using MailManager.Application.Abstraction.Config;
using MailManager.Application.Services.MailChimp;
using MailManager.Application.Services.MockContacts;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using Refit;
namespace mail_manager.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration) {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new Asp.Versioning.ApiVersion(1);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
        }

        public static void AddRefitConfiguration(this IServiceCollection services, IConfiguration configuration) {
            
            services.AddTransient<MailChimpDelegateHandler>();

            //var sourcesConfig = configuration.GetSection("Sources").Value;
            //var sources = JsonConvert.DeserializeObject<IEnumerable<SourcesItem>>(sourcesConfig); 


            services.AddRefitClient<IMockContactsApi>()
            .ConfigureHttpClient(x =>
            {
                 //x.BaseAddress = new Uri(sources.First(c=>c.Name== "Mock-API").Url);
                 x.BaseAddress = new Uri("https://challenge.trio.dev/api/v1");
            });



            services.AddRefitClient<IMailChimpApi>()
            .ConfigureHttpClient(x =>
            {
                //x.BaseAddress = new Uri(sources.First(c=>c.Name== "Mailchimp-API").Url);
                x.BaseAddress = new Uri("https://us16.api.mailchimp.com/3.0");
            }).AddHttpMessageHandler<MailChimpDelegateHandler>();
        }
    }
}
