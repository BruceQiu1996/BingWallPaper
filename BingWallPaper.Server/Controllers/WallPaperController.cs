using BingWallPaper.Server.Dto;
using BingWallPaper.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BingWallPaper.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WallPaperController : ControllerBase
    {
        private readonly WallPaperService _wallPaperService;
        private readonly ILogger<WallPaperController> _logger;
        public WallPaperController(WallPaperService wallPaperService, ILogger<WallPaperController> logger)
        {
            _wallPaperService = wallPaperService;
            _logger = logger;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetByIdAsync(string id)
        {
            try
            {
                var result = await _wallPaperService.GetWallPaperById(id);

                return result.ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem();
            }
        }
    }
}
