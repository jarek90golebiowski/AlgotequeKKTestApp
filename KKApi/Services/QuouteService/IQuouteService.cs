using KKApi.Models.DTOs;

namespace KKApi.Services.QuouteService
{
    public interface IQuouteService
    {
        Task<List<QuoteResponseDto>> GetCalculatedQuotes(Dictionary<string, int> topics);
    }
}