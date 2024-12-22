using KKApi.Models;
using KKApi.Services.ProviderService;
using KKApi.Services.QuouteService;
using Moq;

namespace KKApiTest
{
    public class QuoteServiceTests
    {
        private readonly Mock<IProviderService> _providerServiceMock;
        private readonly QuoteService _quoteService;

        public QuoteServiceTests()
        {
            _providerServiceMock = new Mock<IProviderService>();
            _quoteService = new QuoteService(_providerServiceMock.Object);
        }

        [Fact]
        public async Task GetCalculatedQuotes_ShouldReturnCorrectQuotes_WhenTopicsMatch()
        {
            // Arrange
            var requestTopics = new Dictionary<string, int>
            {
                { "math", 50 },
                { "science", 30 },
                { "biology", 20 },
                { "chemistry", 40 },
                { "english", 15 }
            };

            var providers = new List<Provider>
            {
                new Provider { Name = "Provider_A", Topics = new List<string> { "math", "science" } },
                new Provider { Name = "Provider_B", Topics = new List<string> { "reading", "science" } },
                new Provider { Name = "Provider_C", Topics = new List<string> { "history", "chemistry" } },
                new Provider { Name = "Provider_C", Topics = new List<string> { "history", "math" } }
            };

            _providerServiceMock.Setup(service => service.GetProviders())
                .ReturnsAsync(providers);

            // Act
            var result = await _quoteService.GetCalculatedQuotes(requestTopics);

            // Assert
            Assert.Equal(4, result.Count);     // all providers should be returned

            Assert.Equal(8, result[0].Quote);  // Provider_A returns 0.10 of total 50 + 30
            Assert.Equal(9, result[1].Quote);  // Provider_B returns 0.30 of 30 third-highest topic
            Assert.Equal(10, result[2].Quote); // Provider_C returns 0.25 of 40 second-highest topic
            Assert.Equal(10, result[3].Quote); // Provider_D returns 0.20 of 50 highest topic
        }

        [Fact]
        public async Task GetCalculatedQuotes_ShouldReturnEmptyList_WhenNoMatches()
        {
            // Arrange
            var requestTopics = new Dictionary<string, int>
            {
                { "music", 50 },
                { "biology", 30 }
            };

            var providers = new List<Provider>
            {
                new Provider { Name = "Provider_A", Topics = new List<string> { "math", "science" } },
                new Provider { Name = "Provider_B", Topics = new List<string> { "reading", "science" } },
                new Provider { Name = "Provider_C", Topics = new List<string> { "history", "math" } }
            };

            _providerServiceMock.Setup(service => service.GetProviders())
                .ReturnsAsync(providers);

            // Act
            var result = await _quoteService.GetCalculatedQuotes(requestTopics);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCalculatedQuotes_ShouldReturnOnly_WhenTopicsMatch()
        {
            // Arrange
            var requestTopics = new Dictionary<string, int>
            {
                { "math", 50 },
                { "science", 30 },
                { "biology", 20 }
            };

            var providers = new List<Provider>
            {
                new Provider { Name = "Provider_A", Topics = new List<string> { "math", "science" } },
                new Provider { Name = "Provider_B", Topics = new List<string> { "reading", "science" } },
                new Provider { Name = "Provider_C", Topics = new List<string> { "chemistry", "english" } }
            };

            _providerServiceMock.Setup(service => service.GetProviders())
                .ReturnsAsync(providers);

            // Act
            var result = await _quoteService.GetCalculatedQuotes(requestTopics);

            // Assert
            Assert.Equal(2, result.Count);     // only matching providers should be returned
        }
    }
}