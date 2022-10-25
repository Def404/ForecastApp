using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ForecastApp.categories;
using ForecastApp.products;
using ForecastApp.statistic;
using LiveCharts;
using LiveCharts.Wpf;

namespace ForecastApp.main_window.pages;

public partial class StatisticPage : UserControl{
    
    private readonly DbModuleProduct _moduleProduct = new DbModuleProduct();
    private readonly DbModuleCategory _moduleCategory = new DbModuleCategory();
    private readonly DbModuleStatistic _moduleStatistic = new DbModuleStatistic();
    
    public SeriesCollection SeriesCollection { get; set; }
    public string[] Labels { get; set; }
    public Func<int, string> YFormatter { get; set; }
    
    public StatisticPage(){
        InitializeComponent();
    }
    
    private void StatisticPage_OnLoaded(object sender, RoutedEventArgs e){
        
        var categories = _moduleCategory.GetCategoryList();
        CategoryDiagramCmbBox.ItemsSource = categories;
        
        string date = DateTime.Now.ToString("dd-MM-yyyy");
        
        LoadAllStatistic(date, date);
    }

    private void DayAllStatBtn_OnClick(object sender, RoutedEventArgs e){
        
        string date = DateTime.Now.ToString("dd-MM-yyyy");
        
        LoadAllStatistic(date, date);
    }

    private void MonthAllStatBtn_OnClick(object sender, RoutedEventArgs e){
        
        DateTime dateTimeNow = DateTime.Now;
        string endDate =  dateTimeNow.ToString("dd-MM-yyyy");
        string startDate = new DateTime(dateTimeNow.Year, dateTimeNow.Month - 1, dateTimeNow.Day).ToString("dd-MM-yyyy");
        
        LoadAllStatistic(startDate, endDate);
    }

    private void YearAllStatBtn_OnClick(object sender, RoutedEventArgs e){
        
        DateTime dateTimeNow = DateTime.Now;
        string endDate =  dateTimeNow.ToString("dd-MM-yyyy");
        string startDate = new DateTime(dateTimeNow.Year - 1, dateTimeNow.Month , dateTimeNow.Day).ToString("dd-MM-yyyy");
        
        LoadAllStatistic(startDate, endDate);
    }

    private void AllTimeAllStatBtn_OnClick(object sender, RoutedEventArgs e){
        
        string startDate = DateTime.MinValue.ToString("dd-MM-yyyy");
        string endDate = DateTime.MaxValue.ToString("dd-MM-yyyy");
        
        LoadAllStatistic(startDate, endDate);
    }
    
    private void LoadAllStatistic(string startData, string endData){
        
        List<AllStatistic> alList = _moduleStatistic.GetAllStatList(startData, endData);

        string productName = "";
        int sumCnt = 0;
        decimal sumPrice = 0;
        int cntParam = 0;

        foreach (var statistic in alList){

            if (statistic.SumCnt >= cntParam){
                productName = statistic.ProductName;
                cntParam = statistic.SumCnt;
            }
                

            sumCnt += statistic.SumCnt;
            sumPrice += statistic.SumPrice;
        }

        TopProductNameTxtBlock.Text = productName;
        SumPriceTxtBlock.Text = sumPrice +" ₽";
        CntSumTxtBlock.Text = sumCnt.ToString();
    }

    private void BestSortCmbBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e){
        
       var selectedItem = (TextBlock)BestSortCmbBox.SelectedItem;

       if (selectedItem.Text == "по количеству"){
           BestProductsDataGrid.ItemsSource = _moduleStatistic.GetBestCntSaleProductList();
       }
       else{
           BestProductsDataGrid.ItemsSource = _moduleStatistic.GetBestPriceSaleProductList();
       }
    }

    private void CreatedDiagramBtn_OnClick(object sender, RoutedEventArgs e){
        if (CategoryDiagramCmbBox.SelectedIndex == -1 ||
            ProductDiagramCmbBox.SelectedIndex == -1){
            MessageBox.Show("Укажите товар");
            return;
        }

        var product = (Product)ProductDiagramCmbBox.SelectedItem;
        List<SaleProduct> saleProducts = _moduleStatistic.GetSaleProducts(product.Name);

        var maxDate = saleProducts[0].Date;
        var minDate = saleProducts[^1].Date;

        for (DateTime dateTime = minDate; dateTime<=maxDate;  dateTime = dateTime.AddMonths(1)){
            
            int check = 0;
            
            foreach (var saleProduct in saleProducts){
                
                if (dateTime == saleProduct.Date)
                    check++;
            }
            
            if (check == 0)
                saleProducts.Add(new SaleProduct(dateTime, 0));
        }

        List<SaleProduct> sortSaleProduct = saleProducts.OrderBy(d => d.Date).ToList();

        int[] masInt = new int[sortSaleProduct.Count];
        int i = 0;

        string[] masDate = new string[sortSaleProduct.Count];
        
        foreach (var saleProduct in sortSaleProduct){
            masInt[i] = saleProduct.CntProduct;
            masDate[i] = saleProduct.Date.ToString("Y");
            i++;
        }

        SeriesCollection = new SeriesCollection{
            new LineSeries{
                Title = product.Name,
                Values = new ChartValues<int>(masInt),
                Fill = Brushes.Transparent,
                Stroke = Brushes.Red
            }
        };

        Labels = masDate;
        YFormatter = value => value.ToString();

        DataContext = this;
    }

    private void CategoryDiagramCmbBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e){
        
        var category = (Category)CategoryDiagramCmbBox.SelectedItem;
        if (category is not null){
            var categoryName = category.Name;
            ProductDiagramCmbBox.ItemsSource = _moduleProduct.GetProductsOfCatList(categoryName);
        }
    }
}