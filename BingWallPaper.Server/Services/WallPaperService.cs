using BingWallPaper.Common.Dto;
using BingWallPaper.Server.Database;
using BingWallPaper.Server.Database.Extension;
using BingWallPaper.Server.Dto;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BingWallPaper.Server.Services
{
    public class WallPaperService : IAppService
    {
        private readonly BingwallpaperContext _bingWallPaperContext;
        private readonly string _hostPrefix = "https://cn.bing.com";

        public WallPaperService(BingwallpaperContext bingWallPaperContext)
        {
            _bingWallPaperContext = bingWallPaperContext;
        }

        public async Task<ServiceResult<WallPaperDetailDto>> GetWallPaperById(string id, bool us)
        {
            if (!us)
            {
                var wallpaper = await _bingWallPaperContext.WallpaperCns.FirstOrDefaultAsync(x => x.Id == id);
                if (wallpaper == null)
                    return new ServiceResult<WallPaperDetailDto>(HttpStatusCode.NotFound, "壁纸数据异常");

                return new ServiceResult<WallPaperDetailDto>(wallpaper.ToDetailDto_cn());
            }
            else
            {
                var wallpaper = await _bingWallPaperContext.Wallpapers.FirstOrDefaultAsync(x => x.Id == id);
                if (wallpaper == null)
                    return new ServiceResult<WallPaperDetailDto>(HttpStatusCode.NotFound, "壁纸数据异常");

                return new ServiceResult<WallPaperDetailDto>(wallpaper.ToDetailDto());
            }
        }

        public async Task<ServiceResult<IEnumerable<WallPaperDetailDto>>> GetWallsAsync(int page = 1, int pageCount = 20, string key = null, bool dateDesc = true, bool isUs = false)
        {
            if (isUs)
            {
                var query = _bingWallPaperContext.Wallpapers.AsQueryable();
                if (!string.IsNullOrEmpty(key))
                {
                    query = query.Where(x => x.Desc.Contains(key));
                }
                query = dateDesc ? query.OrderByDescending(x => x.Date) : query.OrderBy(x => x.Date);
                query = query.Skip(pageCount * (page - 1)).Take(pageCount);
                var data = await query.ToListAsync();
                var result = data.Select(x =>
                {
                    var dto = x.ToDetailDto();
                    dto.Image = x.UrlBase + "_UHD.jpg&pid=hp&w=280&h=180&rs=1&c=4";

                    return dto;
                });

                return new ServiceResult<IEnumerable<WallPaperDetailDto>>(result);
            }
            else
            {
                var query = _bingWallPaperContext.WallpaperCns.AsQueryable();
                if (!string.IsNullOrEmpty(key))
                {
                    query = query.Where(x => x.Desc.Contains(key));
                }

                query = dateDesc ? query.OrderByDescending(x => x.Date) : query.OrderBy(x => x.Date);
                query = query.Skip(pageCount * (page - 1)).Take(pageCount);
                var data = await query.ToListAsync();
                var result = data.Select(x =>
                {
                    var dto = x.ToDetailDto_cn();
                    dto.Image = x.UrlBase + "_UHD.jpg&pid=hp&w=280&h=180&rs=1&c=4";

                    return dto;
                });

                return new ServiceResult<IEnumerable<WallPaperDetailDto>>(result);
            }
        }
    }
}
