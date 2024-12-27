using BingWallPaper.Server.Dto;
using BingWallPaper.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BingWallPaper.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WallPapersController : ControllerBase
    {
        private readonly WallPaperService _wallPaperService;
        private readonly ILogger<WallPapersController> _logger;
        public WallPapersController(WallPaperService wallPaperService, ILogger<WallPapersController> logger)
        {
            _wallPaperService = wallPaperService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync(int page = 1, int pageSize = 20, string key = null, bool desc = true)
        {
            try
            {
                var result =
                    await _wallPaperService.GetWallsAsync(page, pageSize, key, desc);

                return result.ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem();
            }
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetByIdAsync(string id, bool cn)
        {
            try
            {
                var result = await _wallPaperService.GetWallPaperById(id, cn);

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
