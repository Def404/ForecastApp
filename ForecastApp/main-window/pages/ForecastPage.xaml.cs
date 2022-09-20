using System.Windows;
using System.Windows.Controls;
using ForecastApp.categories;
using ForecastApp.products;

namespace ForecastApp.main_window.pages;

public partial class ForecastPage : UserControl{
    private readonly DbModuleCategory _moduleCategory = new DbModuleCategory();
    private readonly DbModuleProduct _moduleProduct = new DbModuleProduct();
    public ForecastPage(){
        InitializeComponent();
    }

    private void ForecastPage_OnLoaded(object sender, RoutedEventArgs e){

        var categories = _moduleCategory.GetCategoryList();
        CategoryListCmbBox.ItemsSource = categories;
    }

    private void CategoryListCmbBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e){

        var category = (Category)CategoryListCmbBox.SelectedItem;
        var categoryName = category.Name;
        ProductNameListCmbBox.ItemsSource = _moduleProduct.GetProductsOfCatList(categoryName);
    }
}