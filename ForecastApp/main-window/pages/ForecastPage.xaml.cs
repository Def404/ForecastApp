﻿using System;
using System.Windows;
using System.Windows.Controls;
using ForecastApp.categories;
using ForecastApp.products;

namespace ForecastApp.main_window.pages;

public partial class ForecastPage : UserControl{
    private readonly DbModuleCategory _moduleCategory = new DbModuleCategory();
    private readonly DbModuleProduct _moduleProduct = new DbModuleProduct();
    private readonly DbModuleForecast _moduleForecast = new DbModuleForecast();
    private readonly ForecastingMethod _forecastingMethod = new ForecastingMethod();
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

    private void GetForecastBtn_OnClick(object sender, RoutedEventArgs e){

        if (CategoryListCmbBox.SelectedIndex == -1 || ProductNameListCmbBox.SelectedIndex == -1 ||
            ForecastMethodListCmbBox.SelectedIndex == -1){
            MessageBox.Show("Выбраны не все поля!");
            return;
        }
        
        var category = ((Category)CategoryListCmbBox.SelectedItem).Name;
        var product = ((Product)ProductNameListCmbBox.SelectedItem).Name;

        var productCntList = _moduleForecast.GetProductCntMonths(product, category);
        
        if (productCntList.Count == 0){
            MessageBox.Show("Не хватает данные для получения прогноза\nНужно минимум три записи за 10 месяцев");
            return;
        }
        
        var firstDate = DateTime.Now;
        var endDate = Convert.ToDateTime(productCntList[^1].Date);
        var monthNumber = Math.Abs(firstDate.Month - endDate.Month + (firstDate.Year - endDate.Year) +1);
        
        switch (ForecastMethodListCmbBox.SelectedIndex){
            case 0:
                CntProductDataGrid.ItemsSource = productCntList;
                int forecastValue = _forecastingMethod.ExtrapolationMethod(productCntList);
                if (forecastValue == -1){
                    MessageBox.Show("Не хватает данные для получения прогноза\nНужно минимум три записи за 10 месяцев");
                    return;
                }

                var nextDate = firstDate.AddMonths(1);
                
                string infoText =
                    $"Товар: {product}\nКатегория: {category}\nМетод прогнозирования: {ForecastMethodListCmbBox.Text}\nПериод прогнозирования: {monthNumber}  мес.";
                string forecastText = $"В следущем месяце ({nextDate.Month}.{nextDate.Year}) возможо будет продано {forecastValue} позиций товара";
                
                InfoTxtBlock.Text = infoText;
                ForecastResultTxtBlk.Text = forecastText;
                break;

        }
    }
}