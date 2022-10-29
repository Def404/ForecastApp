using System;
using System.Collections.Generic;
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
        if (category is not null){
            var categoryId = category.Id;
            ProductNameListCmbBox.ItemsSource = _moduleProduct.GetProductsOfCatList(categoryId);
        }
    }

    private void GetForecastBtn_OnClick(object sender, RoutedEventArgs e){

        if (CategoryListCmbBox.SelectedIndex == -1 || ProductNameListCmbBox.SelectedIndex == -1 ||
            ForecastMethodListCmbBox.SelectedIndex == -1){
            MessageBox.Show("Выбраны не все поля!");
            return;
        }
        
        var category = Convert.ToInt32(((Product)ProductNameListCmbBox.SelectedItem).Category);
        var product = ((Product)ProductNameListCmbBox.SelectedItem).Name;

        List<ProductCntMonth> productCntList;
        
        switch (ForecastMethodListCmbBox.SelectedIndex){
            case 0:
                productCntList =  _moduleForecast.GetProductCntMonths(product, category);
                
                if (productCntList.Count == 0){
                    MessageBox.Show("Не хватает данные для получения прогноза");
                    return;
                }
                
                int forecastValue  = _forecastingMethod.ExtrapolationMethod(productCntList);

                if (forecastValue == -1){
                    MessageBox.Show("Не хватает данные для получения прогноза\nНужно минимум три записи за 10 месяцев");
                    return;
                }
                
                PrintResult(forecastValue, productCntList);
                break;
            
            case 1:
                productCntList = _moduleForecast.GetProductCntAllTime(product, category);
                
                if (productCntList.Count == 0){
                    MessageBox.Show("Не хватает данные для получения прогноза");
                    return;
                }
                
                int forecastValue1  = _forecastingMethod.RegressionMethod(productCntList);
                
                if (forecastValue1 == -1){
                    MessageBox.Show("Не хватает данные для получения прогноза\nНужно минимум 15 записей за все время");
                    return;
                }
                
                PrintResult(forecastValue1, productCntList);
                
                break;

        }
    }

    private void PrintResult(int forecastValue, List<ProductCntMonth> productCntMonths){
        
        var category = ((Category)CategoryListCmbBox.SelectedItem).Name;
        var product = ((Product)ProductNameListCmbBox.SelectedItem).Name;
        
        var firstDate = DateTime.Now;
        var endDate = Convert.ToDateTime(productCntMonths[^1].Date);
        var monthNumber = Math.Abs(firstDate.Month - endDate.Month + 12 * (firstDate.Year - endDate.Year) + 1);
        
        CntProductDataGrid.ItemsSource = productCntMonths;

        var nextDate = DateTime.Now.AddMonths(1);
        
        string infoText = $"Товар: {product}\nКатегория: {category}\nМетод прогнозирования: {ForecastMethodListCmbBox.Text}\nПериод прогнозирования: {monthNumber}  мес.";
        string forecastText = $"В следущем месяце ({nextDate.Month}.{nextDate.Year}) возможо будет продано {forecastValue} позиций товара";
                
        InfoTxtBlock.Text = infoText;
        ForecastResultTxtBlk.Text = forecastText;

        ProductNameListCmbBox.SelectedIndex = -1;
        CategoryListCmbBox.SelectedIndex = -1;
        ForecastMethodListCmbBox.SelectedIndex = -1;

    }
}