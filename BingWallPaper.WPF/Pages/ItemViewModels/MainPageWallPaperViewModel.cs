using BingWallPaper.Common.Dto;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BingWallPaper.WPF.Pages.ItemViewModels
{
    public class MainPageWallPaperViewModel : ObservableObject
    {
        public string Id { get; set; }
        public string Desc { get; set; }
        public string Date { get; set; }
        public string ImageSource { get; set; }

        public MainPageWallPaperViewModel(string id, string desc, string date, string imageSource)
        {
            Id = id;
            Desc = desc;
            Date = date;
            ImageSource = imageSource;
        }

        public static MainPageWallPaperViewModel FromDto(WallPaperDetailDto wallPaperDetailDto)
        {
            return new MainPageWallPaperViewModel(wallPaperDetailDto.Id,
                wallPaperDetailDto.Desc,
                wallPaperDetailDto.Date!.Value.ToString("yyyy-MM-dd"),
                wallPaperDetailDto.Image);
        }
    }
}
