﻿using System;
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

        if (_moduleProduct.CheckExistence(productName, category)){
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
        _moduleProduct.DelProduct(category.Name, product.Name);

        DelCategoryListCmbBox.SelectedIndex = -1;
        DelProductListCmbBox.SelectedIndex = -1;
        
        UpdateForm();
    }

    private void UpdateForm(){
        var products = _moduleProduct.GetProductList();
        ProductDataGrid.ItemsSource = products;
    }

    private void DelCategoryListCmbBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e){
     
        var category = (Category)DelCategoryListCmbBox.SelectedItem;
        if (category is not null){
            var categoryName = category.Name;
            DelProductListCmbBox.ItemsSource = _moduleProduct.GetProductsOfCatList(categoryName);
        }
    }
}