using BingWallPaper.Server.Database;
using System.Net;
using BingWallPaper.Server.Dto;
using Microsoft.EntityFrameworkCore;

namespace BingWallPaper.Server.Services
{
    public class WallPaperService : IAppService
    {
        private readonly BingWallPaperContext _bingWallPaperContext;

        public WallPaperService(BingWallPaperContext bingWallPaperContext)
        {
            _bingWallPaperContext = bingWallPaperContext;
        }

        public async Task<ServiceResult> GetWallPaperById(string id)
        {
            var user = await _bingWallPaperContext.Wallpapers.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return new ServiceResult(HttpStatusCode.BadRequest, "用户异常");

           

            return new ServiceResult();
        }
    }
}
