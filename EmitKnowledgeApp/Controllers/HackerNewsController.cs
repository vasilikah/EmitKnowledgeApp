using EmitKnowledgeApp.Models;
using EmitKnowledgeApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

        [HttpGet("GetNewsSortedByNewestToOldest")]
        public async Task<ActionResult> GetNewsSortedByNewestToOldest()
        {
            return Ok(await _hackerNewsService.GetNewsSortedByNewestToOldest());
        }

        [HttpGet("GetHotNews")]
        public async Task<ActionResult> GetHotNews()
        {
            return Ok(await _hackerNewsService.GetHotNews());
        }
    }
}