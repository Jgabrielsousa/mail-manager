using MailManager.Application.Services.MailChimp;
using MailManager.Application.Services.MailChimp.Reponses;
using MailManager.Application.Services.MailChimp.Requests;
using MailManager.Application.Services.MockContacts;
using MailManager.Application.Services.MockContacts.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;

namespace MailManager.Integration.Test
{
    public class MailApplicationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public MailApplicationTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                   
                    var serviceMockContactsApi = services.SingleOrDefault(d => d.ServiceType == typeof(IMockContactsApi));
                    if (serviceMockContactsApi != null)
                        services.Remove(serviceMockContactsApi);

                    var mockContactApi = new Mock<IMockContactsApi>();
                    mockContactApi
                        .Setup(api => api.Get())
                        .ReturnsAsync(() => new List<ContactsResponse>() { new ContactsResponse() { FirstName="Joao",LastName = "Sousa", Id = Guid.NewGuid().ToString()} });

                    services.AddSingleton(mockContactApi.Object);



                    var serviceMailChimpApi = services.SingleOrDefault(d => d.ServiceType == typeof(IMailChimpApi));
                    if (serviceMailChimpApi != null)
                        services.Remove(serviceMailChimpApi);


                    var mockMailChimpApi = new Mock<IMailChimpApi>();
                    mockMailChimpApi
                    .Setup(api => api.Post(It.IsAny<string>(), It.IsAny<ContactRequest>()))
                    .ReturnsAsync(() => new ContactResponse() { Id= Guid.NewGuid().ToString() });

                    services.AddSingleton(mockMailChimpApi.Object);
                });
            }).CreateClient();
        }


        [Fact]
        public async Task ContactEndpoint_Returns200()
        {
            // Arrange
            var expected = HttpStatusCode.OK;

            // Act
            var response = await _client.GetAsync("/api/v1/contacts/sync");

            // Assert
            Assert.Equal(expected, response.StatusCode);
        }

    }
}