using System.Windows;
using System.Windows.Controls;
using ForecastApp.main_window.pages.settings_page;

namespace ForecastApp.main_window.pages;

public partial class SettingsPage : UserControl{
    
    private readonly CategorySettingsPage _categorySettingsPage = new CategorySettingsPage();
    private readonly ProductSettingsPage _productSettingsPage = new ProductSettingsPage();
    private readonly ProfileSettingsPage _profileSettingsPage = new ProfileSettingsPage();
    public SettingsPage(){
        InitializeComponent();
    }

    private void CategorySettingsPageBtn_OnClick(object sender, RoutedEventArgs e){
        RenderSubPage.Children.Clear();
        RenderSubPage.Children.Add(_categorySettingsPage);
    }

    private void ProductSettingsPageBtn_OnClick(object sender, RoutedEventArgs e){
        RenderSubPage.Children.Clear();
        RenderSubPage.Children.Add(_productSettingsPage);
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e){
        RenderSubPage.Children.Clear();
        RenderSubPage.Children.Add(_profileSettingsPage);
    }
}