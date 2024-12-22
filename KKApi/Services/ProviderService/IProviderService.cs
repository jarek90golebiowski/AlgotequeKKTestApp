using KKApi.Models;

namespace KKApi.Services.ProviderService
{
    public interface IProviderService
    {
        Task<IReadOnlyList<Provider>> GetProviders();
    }
}
