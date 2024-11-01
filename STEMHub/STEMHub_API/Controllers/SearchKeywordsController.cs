using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Services.Interfaces;
using STEMHub.STEMHub_Services.Services.Service;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchKeywordsController : ControllerBase
    {
        public ISearchService _searchService;

        public SearchKeywordsController (ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("top-keywords")]
        public async Task<IActionResult> GetTopKeywords()
        {
            var topKeywords = await _searchService.GetTopKeywordsAsync();
            return Ok(topKeywords);
        }
    }
}
