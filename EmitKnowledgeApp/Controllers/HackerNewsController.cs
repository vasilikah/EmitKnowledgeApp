using EmitKnowledgeApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmitKnowledgeApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService;

        public HackerNewsController(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }

        [HttpGet("GetTopNews")]
        public async Task<ActionResult> GetTopNews()
        {
            return Ok(await _hackerNewsService.GetTopNews());
        }
    }
}
