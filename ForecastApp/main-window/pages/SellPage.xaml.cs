using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ForecastApp.database.categories;
using ForecastApp.database.products;
using ForecastApp.database.sales;

namespace ForecastApp.main_window.pages;

public partial class SellPage : UserControl{
    
    private readonly DbModuleSale _moduleSale = new DbModuleSale();
    private readonly DbModuleCategory _moduleCategory = new DbModuleCategory();
    private readonly DbModuleProduct _moduleProduct = new DbModuleProduct();
    public SellPage(){
        InitializeComponent();
    }

    private void SellPage_OnLoaded(object sender, RoutedEventArgs e){
        var sales = _moduleSale.GetSaleList();
        SaleListDataGrid.ItemsSource = sales;

        var categories = _moduleCategory.GetCategoryList();
        CategoryListCmbBox.ItemsSource = categories;

        SaleDatePicker.Language =
            XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag);
        SaleDatePicker.DisplayDateEnd = DateTime.Now;
    }

    private void CategoryListCmbBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e){

        var category = (Category)CategoryListCmbBox.SelectedItem;
        if (category is not null){
            var categoryId = category.Id;
            ProductListCmnBox.ItemsSource = _moduleProduct.GetProductsOfCatList(categoryId);
        }
    }


    private void AddSaleBtn_OnClick(object sender, RoutedEventArgs e){

        var category = (Category)CategoryListCmbBox.SelectedItem;
        var product = (Product)ProductListCmnBox.SelectedItem;
        var cntProduct = 0;
       
        var dateTimeStr = "";

        if (category is null || product is null){
            MessageBox.Show("category or product is null");
            return;
        }

        try{
            cntProduct = Convert.ToInt32(CntProductTxtBox.Text);
            var dateTime = Convert.ToDateTime(SaleDatePicker.Text);
            dateTimeStr = dateTime.ToString("MM-dd-yyyy");
        }
        catch (Exception exception){
            MessageBox.Show(exception.Message);
            return;
        }
        _moduleSale.SetSale(product.Id, cntProduct, dateTimeStr);
        
        ProductListCmnBox.SelectedIndex = -1;
        CategoryListCmbBox.SelectedIndex = -1;
        
        CntProductTxtBox.Clear();
        SaleDatePicker.Text = "";
        
        var sales = _moduleSale.GetSaleList();
        SaleListDataGrid.ItemsSource = sales;
    }
}