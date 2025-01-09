using MailManager.Application.Dtos;
using MailManager.Application.Services.MailChimp;
using MailManager.Application.Services.MailChimp.Reponses;
using MailManager.Application.Services.MailChimp.Requests;
using MailManager.Application.Services.MockContacts;
using MailManager.Application.Services.MockContacts.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace MailManager.Integration.Test.EndPoints.v1
{
    public sealed class ContactsModuleTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;
        private string route = "/api/v1/contacts/sync";

        public ContactsModuleTest(WebApplicationFactory<Program> factory)
        => _factory = factory;

        [Fact]
        public async Task FakeCallIntegrationSyncRoute()
        {
            // Arrange
            var _client = CreateClient(true);
            var expected = HttpStatusCode.OK;
            var syncedUsers = 1;

            // Act
            var response = await _client.GetAsync(route);
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<SyncedContactsDto>(content);

            // Assert
            Assert.Equal(expected, response.StatusCode);
            Assert.Equal(syncedUsers, users.Contacts.Count());
        }

        [Fact]
        public async Task RealCallIntegrationSyncRoute()
        {
            // Arrange
            var _client = CreateClient(false);
            var expected = HttpStatusCode.OK;
            var syncedUsers = 1;

            // Act
            var response = await _client.GetAsync(route);
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<SyncedContactsDto>(content);

            // Assert
            Assert.Equal(expected, response.StatusCode);
            Assert.Equal(syncedUsers, users.Contacts.Count());
        }

        private HttpClient CreateClient(bool fakeCalls)
        {
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    if (fakeCalls)
                    {
                        var serviceMockContactsApi = services.SingleOrDefault(d => d.ServiceType == typeof(IMockContactsApi));
                        if (serviceMockContactsApi != null)
                            services.Remove(serviceMockContactsApi);

                        var mockContactApi = new Mock<IMockContactsApi>();
                        mockContactApi
                            .Setup(api => api.Get())
                            .ReturnsAsync(() => new List<ContactsResponse>() { new ContactsResponse() { Email = "jgabrielsousa@gmail.com", FirstName = "Joao", LastName = "Sousa", Id = Guid.NewGuid().ToString() } });

                        services.AddSingleton(mockContactApi.Object);

                        var serviceMailChimpApi = services.SingleOrDefault(d => d.ServiceType == typeof(IMailChimpApi));
                        if (serviceMailChimpApi != null)
                            services.Remove(serviceMailChimpApi);


                        var mockMailChimpApi = new Mock<IMailChimpApi>();
                        mockMailChimpApi
                        .Setup(api => api.Post(It.IsAny<string>(), It.IsAny<ContactRequest>()))
                        .ReturnsAsync(() => new ContactResponse() { Id = Guid.NewGuid().ToString() });

                        services.AddSingleton(mockMailChimpApi.Object);
                    }
                });
            }).CreateClient();

            return _client;
        }

       
    }
}