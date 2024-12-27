using BingWallPaper.Common.Dto;
using BingWallPaper.WPF.Helpers;
using BingWallPaper.WPF.Pages.ItemViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace BingWallPaper.WPF.Pages
{
    public class MainPageViewModel : ObservableObject
    {
        private ObservableCollection<MainPageWallPaperViewModel> _paperViewModels;
        public ObservableCollection<MainPageWallPaperViewModel> PaperViewModels
        {
            get => _paperViewModels;
            set => SetProperty(ref _paperViewModels, value);
        }

        public AsyncRelayCommand LoadAsyncCommand { get; set; }

        private readonly HttpRequest _httpRequest;
        public MainPageViewModel(HttpRequest httpRequest)
        {
            _httpRequest  = httpRequest;
            PaperViewModels = new ObservableCollection<MainPageWallPaperViewModel>();
            LoadAsyncCommand = new AsyncRelayCommand(LoadAsync);
        }

        private async Task LoadAsync() 
        {
            PaperViewModels.Clear();
            try
            {
                var resp = await _httpRequest.GetAsync($"{HttpRequestUrls.WallPaper}?page = 1");
                if (resp.IsSuccessStatusCode) 
                {
                    var data = 
                        JsonSerializer.Deserialize<IEnumerable<WallPaperDetailDto>>(await resp.Content.ReadAsStringAsync(),HttpRequest._jsonSerializerOptions);

                    foreach (var item in data) 
                    {
                        PaperViewModels.Add(MainPageWallPaperViewModel.FromDto(item));
                    }
                }
            }
            catch (Exception ex) 
            {
                
            }
        }

        public async Task FetchWallPaperAsync() 
        {
            
        }
    }
}
