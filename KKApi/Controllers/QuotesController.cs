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
        private readonly ILogger<QuotesController> _logger;

        public QuotesController(
            IQuouteService quouteService,
            ILogger<QuotesController> logger)
        {
            _quouteService = quouteService;
            _logger = logger;
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