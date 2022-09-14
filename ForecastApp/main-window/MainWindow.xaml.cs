using System.Windows;
using ForecastApp.main_window.pages;


namespace ForecastApp{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window{

        private readonly MainPage _mainPage = new MainPage();
        private readonly SellPage _sellPage = new SellPage();
        private readonly ForecastPage _forecastPage = new ForecastPage();
        private readonly StatisticPage _statisticPage = new StatisticPage();
        private readonly ProfilePage _profilePage = new ProfilePage();
        
        public MainWindow(){
            InitializeComponent();
        }

        private void MainPageBtn_OnClick(object sender, RoutedEventArgs e){
            RenderPage.Children.Clear();
            RenderPage.Children.Add(_mainPage);
        }

        private void SellPageBtn_OnClick(object sender, RoutedEventArgs e){
            RenderPage.Children.Clear();
            RenderPage.Children.Add(_sellPage);
        }

        private void ForecastPageBtn_OnClick(object sender, RoutedEventArgs e){
            RenderPage.Children.Clear();
            RenderPage.Children.Add(_forecastPage);
        }

        private void StatisticPageBtn_OnClick(object sender, RoutedEventArgs e){
            RenderPage.Children.Clear();
            RenderPage.Children.Add(_statisticPage);
        }

        private void ProfilePageBtn_OnClick(object sender, RoutedEventArgs e){
            RenderPage.Children.Clear();
            RenderPage.Children.Add(_profilePage);
        }
    }
}