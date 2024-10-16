using Azure.AISearch.Api.Service.Models;
using Azure.AISearch.Api.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Azure.AISearch.Api.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AISearchController:ControllerBase
    {
        private readonly ISearchServiceAzure _service;
        private readonly ILogger<AISearchController> _logger;

        public AISearchController(ISearchServiceAzure service, ILogger<AISearchController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<AppReviewAI>>> Search([FromQuery] Filter request)
        {
            try
            {
                var res = await _service.Search(request);
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"SqlSearchController::Search Exception: {ex.StackTrace}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetPlatforms")]
        public async Task<ActionResult<IEnumerable<string>>> GetPlatforms()
        {
            try
            {
                var res = await _service.GetPlatforms();
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"SqlSearchController::GetPlatforms Exception: {ex.StackTrace}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetCountries")]
        public async Task<ActionResult<IEnumerable<string>>> GetCountries()
        {
            try
            {
                var res = await _service.GetCountries();
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"SqlSearchController::GetCountries Exception: {ex.StackTrace}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetStars")]
        public async Task<ActionResult<IEnumerable<int>>> GetStars()
        {
            try
            {
                var res = await _service.GetStars();
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"SqlSearchController::GetStars Exception: {ex.StackTrace}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GeLabels")]
        public async Task<ActionResult<IEnumerable<string>>> GeLabels()
        {
            try
            {
                var res = await _service.GeLabels();
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"SqlSearchController::GeLabels Exception: {ex.StackTrace}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
