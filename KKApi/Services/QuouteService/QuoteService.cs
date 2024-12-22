using KKApi.Models.DTOs;
using KKApi.Services.ProviderService;

namespace KKApi.Services.QuouteService
{
    public class QuoteService : IQuouteService
    {
        private readonly IProviderService _providerService;

        public QuoteService(IProviderService providerService)
        {
            _providerService = providerService;
        }

        public async Task<List<QuoteResponseDto>> GetCalculatedQuotes(Dictionary<string, int> requestTopics)
        {
            try
            {
                var results = new List<QuoteResponseDto>();
                var providers = await _providerService.GetProviders();
                var top3Topics = GetTop3Topics(requestTopics);

                foreach (var provider in providers)
                {
                    var quote = CalculateQuote(top3Topics, provider.Topics);

                    if (quote > 0)
                    {
                        results.Add(
                        new QuoteResponseDto
                        {
                            Provider = provider.Name,
                            Quote = quote
                        });
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in quotes calculation.", ex);
            }
        }

        private List<KeyValuePair<string, int>> GetTop3Topics(Dictionary<string, int> requestTopics)
        {
            return requestTopics
                .OrderByDescending(x => x.Value)
                .Take(3)
                .ToList();
        }
        private double CalculateQuote(List<KeyValuePair<string, int>> top3Topics, IReadOnlyList<string> providerTopics)
        {
            var matchingTopics = top3Topics
                .Where(topic => providerTopics.Contains(topic.Key))
                .ToList();

            return matchingTopics.Count switch
            {
                2 => GetDoubleTopicMatch(matchingTopics),
                1 => GetSingleTopicMatch(top3Topics, matchingTopics),
                _ => 0
            };
        }

        private double GetDoubleTopicMatch(IEnumerable<KeyValuePair<string, int>> matchingTopics)
        {
            return matchingTopics.Sum(x => x.Value) * 0.1;
        }

        private double GetSingleTopicMatch(List<KeyValuePair<string, int>> top3Topics, IEnumerable<KeyValuePair<string, int>> matchingTopics)
        {
            var matchingTopic = matchingTopics.First();

            // reflect importance of the topic
            var topicIndex = top3Topics.IndexOf(matchingTopic);

            var singleTopicMatchFactor = topicIndex switch
            {
                0 => 0.2,
                1 => 0.25,
                2 => 0.3,
                _ => throw new NotImplementedException()
            };

            return matchingTopic.Value * singleTopicMatchFactor;
        }
    }
}
