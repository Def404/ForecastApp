using System.Windows;
using System.Windows.Controls;
using ForecastApp.categories;
using ForecastApp.products;

namespace ForecastApp.main_window.pages.settings_page;

public partial class ProductSettingsPage : UserControl{

    private readonly DbModuleProduct _moduleProduct = new DbModuleProduct();
    private readonly DbModuleCategory _moduleCategory = new DbModuleCategory();
    public ProductSettingsPage(){
        InitializeComponent();
    }

    private void ProductSettingsPage_OnLoaded(object sender, RoutedEventArgs e){

        var products = _moduleProduct.GetProductList();
        ProductDataGrid.ItemsSource = products;

        var categories = _moduleCategory.GetCategoryList();
        AddCategoryListCmbBox.ItemsSource = categories;
        DelCategoryListCmbBox.ItemsSource = categories;
    }

    private void AddProductBtn_OnClick(object sender, RoutedEventArgs e){
        
    }

    private void DelProductBtn_OnClick(object sender, RoutedEventArgs e){
        throw new System.NotImplementedException();
    }
}