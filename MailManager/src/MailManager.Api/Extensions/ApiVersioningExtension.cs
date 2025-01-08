using Asp.Versioning;
using Asp.Versioning.Builder;

namespace mail_manager.Extensions
{
    public static class ApiVersioningExtension
    {
        public static ApiVersionSet GetApiVersionSet(this IEndpointRouteBuilder app) =>
                app.NewApiVersionSet()
                   .HasApiVersion(new ApiVersion(1))
                   .ReportApiVersions()
                   .Build();

        public static RouteGroupBuilder VersionedGroup(this IEndpointRouteBuilder app) =>
                app.MapGroup("/api/v{version:apiVersion}")
                   .WithApiVersionSet(app.GetApiVersionSet());

    }
}
