using MailManager.Application.Abstraction;
using MailManager.Application.Abstraction.Config;
using MailManager.Application.Dtos;
using MailManager.Application.Services.MailChimp;
using MailManager.Application.Services.MailChimp.Reponses;
using MailManager.Application.Services.MailChimp.Requests;
using MailManager.Application.Services.MockContacts;
using MailManager.Application.Services.MockContacts.Responses;
using MailManager.Application.UseCases.SyncContacts;
using Microsoft.Extensions.Options;
using Moq;

namespace MailManager.Unit.Test.UseCases
{
    public class SyncContactsHandlerTest
    {
        private Mock<IMailChimpApi> _mailChimpApi;
        private Mock<IMockContactsApi> _mockContactsApi;
        private Mock<IOptions<AppSettings>> _mockAppSettings;

        public SyncContactsHandlerTest()
        {
            _mockAppSettings = new Mock<IOptions<AppSettings>>();
            _mockAppSettings.Setup(c => c.Value).Returns(() => 
            new AppSettings() { Sources = new List<Sources>() 
            { 
                new Sources() { ApiKey = "123",Name=StringConst.ContactDefinition,Url =""}, 
                new Sources() { ApiKey = "abc", Name = StringConst.MailChimpDefinition, Url = "" } 
            } 
            });
        }

        [Fact]
        public async void ShouldNotSyncUsersWhenThereAreNoUsersToSync()
        {
            _mockContactsApi = new Mock<IMockContactsApi>();
            _mockContactsApi
                .Setup(api => api.Get())
                .ReturnsAsync(() => new List<ContactsResponse>() { });

            _mailChimpApi = new Mock<IMailChimpApi>();
            _mailChimpApi
            .Setup(api => api.Post(It.IsAny<string>(), It.IsAny<ContactRequest>()))
            .ReturnsAsync(() => new ContactResponse() { Id = Guid.NewGuid().ToString() });

            // Act
            var handler = GetHandler();
            var result = await handler.Execute(null, CancellationToken.None);

            var resultDto = (SyncedContactsDto)result.Data;
            // Assert
            Assert.Equal(0, resultDto.SyncedContacts);

        }

        [Fact]
        public async void ShouldNotSyncUsersWhenChimpServiceIsOut()
        {
            // Arrange
            _mockContactsApi = new Mock<IMockContactsApi>();
            _mockContactsApi
                .Setup(api => api.Get())
                .ReturnsAsync(() => new List<ContactsResponse>() { new ContactsResponse() { Email = "jgabrielsousa@gmail.com", FirstName = "Joao", LastName = "Sousa", Id = Guid.NewGuid().ToString() } });

            _mailChimpApi = new Mock<IMailChimpApi>();
            _mailChimpApi
            .Setup(api => api.Post(It.IsAny<string>(), It.IsAny<ContactRequest>()))
            .Throws(new Exception());

            // Act
            var handler = GetHandler();
            var result = await handler.Execute(null, CancellationToken.None);

            var resultDto = (SyncedContactsDto)result.Data;
            // Assert
            Assert.Equal(0, resultDto.SyncedContacts);

        }

        [Fact]
        public async void ShouldSyncSuccessfully()
        {
            // Arrange
            _mockContactsApi = new Mock<IMockContactsApi>();
            _mockContactsApi
                .Setup(api => api.Get())
                .ReturnsAsync(() => new List<ContactsResponse>() { new ContactsResponse() { Email = "jgabrielsousa@gmail.com", FirstName = "Joao", LastName = "Sousa", Id = Guid.NewGuid().ToString() } });

            _mailChimpApi = new Mock<IMailChimpApi>();
            _mailChimpApi
            .Setup(api => api.Post(It.IsAny<string>(), It.IsAny<ContactRequest>()))
            .ReturnsAsync(() => new ContactResponse() { Id = Guid.NewGuid().ToString() });

            // Act
            var handler = GetHandler();
            var result = await handler.Execute(null, CancellationToken.None);

            var resultDto = (SyncedContactsDto)result.Data;
            // Assert
            Assert.Equal(1, resultDto.SyncedContacts);

        }

        public SyncContactsHandler GetHandler()
            => new SyncContactsHandler(_mailChimpApi.Object, _mockContactsApi.Object, _mockAppSettings.Object);
    }
}