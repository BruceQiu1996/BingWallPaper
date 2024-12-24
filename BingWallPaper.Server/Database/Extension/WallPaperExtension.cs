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
                HDImage = wallpaper.UrlBase,
                Desc = wallpaper.Desc,
                Date = wallpaper.Date,
            };
        }
    }
}
