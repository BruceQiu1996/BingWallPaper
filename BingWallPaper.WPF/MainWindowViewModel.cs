using BingWallPaper.WPF.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;

namespace BingWallPaper.WPF
{
    internal class MainWindowViewModel : ObservableObject
    {
        private Page _currentPage;
        public Page CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        public MainWindowViewModel(MainPage mainPage)
        {
            CurrentPage = mainPage;
        }
    }
}
