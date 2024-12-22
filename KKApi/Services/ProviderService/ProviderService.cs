using KKApi.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace KKApi.Services.ProviderService
{
    public class ProviderService : IProviderService
    {

        private readonly string _filePath;
        private readonly IMemoryCache _cache;

        public ProviderService(
            IConfiguration configuration,
            IMemoryCache cache)
        {
            _cache = cache;
            _filePath = configuration["ProvidersFilePath"];

            if (string.IsNullOrEmpty(_filePath))
            {
                throw new Exception("Providers file path not found in configuration.");
            }
        }

        public async Task<IReadOnlyList<Provider>> GetProviders()
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"The file with providers was not found: {_filePath}");
            }

            try
            {
                if (!_cache.TryGetValue("provider_topics", out List<Provider> providers))
                {
                    providers = new List<Provider>();
                    string jsonData = await File.ReadAllTextAsync(_filePath);
                    var providerTopicsRoot = JsonSerializer.Deserialize<ProviderTopicsRoot>(jsonData);

                    if (providerTopicsRoot == null)
                    {
                        throw new Exception("Unexpected error on deseriazlization providers.");
                    }

                    foreach (var providerTopic in providerTopicsRoot.ProviderTopics)
                    {
                        providers.Add(
                            new Provider()
                            {
                                Name = providerTopic.Key,
                                Topics = providerTopic.Value.Split('+').ToList()
                            });
                    }

                    _cache.Set("provider_topics", providers, TimeSpan.FromHours(1));
                }

                return providers;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error on reading providers configuration file.", ex);
            }
        }
    }
}