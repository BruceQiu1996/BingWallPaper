using BingWallPaper.Common.Dto;
using BingWallPaper.Server.Database;
using BingWallPaper.Server.Dto;
using Microsoft.EntityFrameworkCore;
using BingWallPaper.Server.Database.Extension;
using System.Net;

namespace BingWallPaper.Server.Services
{
    public class WallPaperService : IAppService
    {
        private readonly BingwallpaperContext _bingWallPaperContext;

        public WallPaperService(BingwallpaperContext bingWallPaperContext)
        {
            _bingWallPaperContext = bingWallPaperContext;
        }

        public async Task<ServiceResult<WallPaperDetailDto>> GetWallPaperById(string id)
        {
            var wallpaper = await _bingWallPaperContext.Wallpapers.FirstOrDefaultAsync(x => x.Id == id);
            if (wallpaper == null)
                return new ServiceResult<WallPaperDetailDto>(HttpStatusCode.NotFound, "壁纸数据异常");

            return new ServiceResult<WallPaperDetailDto>(wallpaper.ToDetailDto());
        }
    }
}
