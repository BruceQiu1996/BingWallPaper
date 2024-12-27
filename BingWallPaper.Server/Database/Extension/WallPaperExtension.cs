using BingWallPaper.Common.Dto;

namespace BingWallPaper.Server.Database.Extension
{
    public static class WallPaperExtension
    {
        public static WallPaperDetailDto ToDetailDto(this Wallpaper wallpaper) 
        {
            return new WallPaperDetailDto()
            {
                Id = wallpaper.Id,
                Desc = wallpaper.Desc,
                Date = wallpaper.Date,
            };
        }

        public static WallPaperDetailDto ToDetailDto_cn(this WallpaperCn wallpaper)
        {
            return new WallPaperDetailDto()
            {
                Id = wallpaper.Id,
                Desc = wallpaper.Desc,
                Date = wallpaper.Date,
            };
        }
    }
}
