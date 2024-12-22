using KKApi.Models.DTOs;
using KKApi.Services.QuouteService;
using Microsoft.AspNetCore.Mvc;

namespace KKApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuotesController : ControllerBase
    {
        private readonly IQuouteService _quouteService;

        public QuotesController(
            IQuouteService quouteService)
        {
            _quouteService = quouteService;
        }

        [HttpPost]
        public async Task<IActionResult> GetQuotes([FromBody] QuoteRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var quotes = await _quouteService.GetCalculatedQuotes(request.Topics);

            return Ok(quotes);
        }
    }
}