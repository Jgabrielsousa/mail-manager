using Asp.Versioning;
using MailManager.Application.Abstraction;
using MailManager.Application.Abstraction.Config;
using MailManager.Application.Services.MailChimp;
using MailManager.Application.Services.MockContacts;
using MailManager.Application.UseCases.Base;
using MailManager.Application.UseCases.SyncContacts;
using Microsoft.Extensions.Options;
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

            services.Configure<AppSettings>(configuration.GetSection(StringConst.SectionDefinition));

            services.AddTransient<MailChimpDelegateHandler>();

            services.AddRefitClient<IMockContactsApi>()
            .ConfigureHttpClient((serviceProvider, httpClient) => 
            {
                var apiSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
                httpClient.BaseAddress = new Uri(apiSettings.Sources.First(c=>c.Name== StringConst.ContactDefinition).Url);
            });

            services.AddRefitClient<IMailChimpApi>()
            .ConfigureHttpClient((serviceProvider, httpClient) =>
            {
                var apiSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
                httpClient.BaseAddress = new Uri(apiSettings.Sources.First(c=>c.Name== StringConst.MailChimpDefinition).Url);

            }).AddHttpMessageHandler<MailChimpDelegateHandler>();
        }

        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<SyncContactsHandler>();
            services.AddScoped(typeof(IUseCaseHandler<,>), typeof(UseCaseHandler<,>));
        }
    }
}
