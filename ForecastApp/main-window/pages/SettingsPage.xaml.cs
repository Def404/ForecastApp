using System.Windows;
using System.Windows.Controls;
using ForecastApp.main_window.pages.settings_page;

namespace ForecastApp.main_window.pages;

public partial class SettingsPage : UserControl{
    
    private static readonly CategorySettingsPage _categorySettingsPage = new CategorySettingsPage();
    private static readonly ProductSettingsPage _productSettingsPage = new ProductSettingsPage();
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
}