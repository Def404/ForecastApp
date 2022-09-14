using System.Windows;
using System.Windows.Controls;

namespace ForecastApp.main_window.pages;

public partial class SettingsPage : UserControl{
    private static readonly CategorySettingsPage _categorySettingsPage = new CategorySettingsPage();
    public SettingsPage(){
        InitializeComponent();
    }

    private void CategorySettingsPageBtn_OnClick(object sender, RoutedEventArgs e){
        RenderSubPage.Children.Clear();
        RenderSubPage.Children.Add(_categorySettingsPage);
    }
}