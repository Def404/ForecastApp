using System;
using System.Windows;
using System.Windows.Controls;
using ForecastApp.database.categories;
using ForecastApp.database.products;

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
        ChangeNameCmbBox.ItemsSource = products;
        ChangePriceCmbBox.ItemsSource = products;

        var categories = _moduleCategory.GetCategoryList();
        AddCategoryListCmbBox.ItemsSource = categories;
        DelCategoryListCmbBox.ItemsSource = categories;
    }

    private void AddProductBtn_OnClick(object sender, RoutedEventArgs e){
        
        string productName = AddProductNameTxtBox.Text.Trim();
        string category = AddCategoryListCmbBox.Text;
        decimal price = 0;

        if (productName.Length <= 0 || category.Length <= 0 || AddPriceTxtBox.Text.Trim().Length <= 0){
            MessageBox.Show("Не все поля указаны");
            return;
        }
        
        try{
            price = Convert.ToDecimal(AddPriceTxtBox.Text.Trim());
        }
        catch (Exception exception){
            MessageBox.Show("Укажите правильно цену");
            return;
        }

        if (_moduleProduct.CheckExistence(productName)){
            MessageBox.Show("Данный товара уже сущесвтвует");
            return;
        }
       
        
        _moduleProduct.SetProduct(productName, category, price);
        
        AddProductNameTxtBox.Clear();
        AddPriceTxtBox.Clear();
        AddCategoryListCmbBox.SelectedIndex = -1;

        UpdateForm();
    }

    private void DelProductBtn_OnClick(object sender, RoutedEventArgs e){
        if (DelCategoryListCmbBox.Text.Length <= 0 || DelProductListCmbBox.Text.Length <= 0){
            MessageBox.Show("Выберете продукт");
            return;
        }

        var category = (Category)DelCategoryListCmbBox.SelectedItem;
        var product = (Product)DelProductListCmbBox.SelectedItem;
        _moduleProduct.DelProduct(product.Name, category.Name);

        DelCategoryListCmbBox.SelectedIndex = -1;
        DelProductListCmbBox.SelectedIndex = -1;
        
        UpdateForm();
    }
    
    private void DelCategoryListCmbBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e){
     
        var category = (Category)DelCategoryListCmbBox.SelectedItem;
        if (category is not null){
            var categoryId = category.Id;
            DelProductListCmbBox.ItemsSource = _moduleProduct.GetProductsOfCatList(categoryId);
        }
    }

    private void ChangeNameProductBtn_OnClick(object sender, RoutedEventArgs e){
        
        if (ChangeNameCmbBox.SelectedIndex == -1 ||
            ChangeNameTxtBox.Text.Trim() == ""){
            MessageBox.Show("Выберете продукт");
            return;
        }

        var product = (Product)ChangeNameCmbBox.SelectedItem;
        _moduleProduct.UpdateProductName(product.Name, ChangeNameTxtBox.Text.Trim());

        ChangeNameCmbBox.SelectedIndex = -1;
        ChangeNameTxtBox.Clear();
        
        UpdateForm();
    }

    private void ChangePriceBtn_OnClick(object sender, RoutedEventArgs e){
        if (ChangePriceCmbBox.SelectedIndex == -1 ||
            ChangePriceTxtBox.Text.Trim() == ""){
            MessageBox.Show("Выберете продукт");
            return;
        }
        
        var product = (Product)ChangePriceCmbBox.SelectedItem;
        _moduleProduct.UpdateProductPrice(product.Name, Convert.ToDecimal(ChangePriceTxtBox.Text.Trim()));

        ChangePriceCmbBox.SelectedIndex = -1;
        ChangePriceTxtBox.Clear();
        
        UpdateForm();
    }
    
    private void UpdateForm(){
        var products = _moduleProduct.GetProductList();
        ProductDataGrid.ItemsSource = products;
        ChangeNameCmbBox.ItemsSource = products;
        ChangePriceCmbBox.ItemsSource = products;
        
    }
}