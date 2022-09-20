using System.Windows;
using System.Windows.Controls;
using ForecastApp.categories;
using ForecastApp.products;
using ForecastApp.sales;

namespace ForecastApp.main_window.pages;

public partial class SellPage : UserControl{
    
    private DbModuleSale _moduleSale = new DbModuleSale();
    private DbModuleCategory _moduleCategory = new DbModuleCategory();
    private DbModuleProduct _moduleProduct = new DbModuleProduct();
    public SellPage(){
        InitializeComponent();
    }

    private void SellPage_OnLoaded(object sender, RoutedEventArgs e){
        var sales = _moduleSale.GetSaleList();
        SaleListDataGrid.ItemsSource = sales;

        var categories = _moduleCategory.GetCategoryList();
        CategoryListCmbBox.ItemsSource = categories;
    }

    private void CategoryListCmbBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e){

        var category = (Category)CategoryListCmbBox.SelectedItem;
        var categoryName = category.Name;
        ProductListCmnBox.ItemsSource = _moduleProduct.GetProductsOfCatList(categoryName);
    }
}