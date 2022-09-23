﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ForecastApp.statistic;

namespace ForecastApp.main_window.pages;

public partial class StatisticPage : UserControl{
    private DbModuleStatistic _moduleStatistic = new DbModuleStatistic();
    public StatisticPage(){
        InitializeComponent();
    }
    
    private void StatisticPage_OnLoaded(object sender, RoutedEventArgs e){
        string date = DateTime.Now.ToString("dd-MM-yyyy");
        
        LoadAllStatistic(date, date);
    }

    private void DayAllStatBtn_OnClick(object sender, RoutedEventArgs e){
        string date = DateTime.Now.ToString("MM-dd-yyyy");
        
        LoadAllStatistic(date, date);
    }

    private void MonthAllStatBtn_OnClick(object sender, RoutedEventArgs e){
        DateTime dateTimeNow = DateTime.Now;
        string startDate =  dateTimeNow.ToString("MM-dd-yyyy");
        string endDate = new DateTime(dateTimeNow.Year, dateTimeNow.Month - 1, dateTimeNow.Day).ToString("MM-dd-yyyy");
        
        LoadAllStatistic(startDate, endDate);
    }

    private void YearAllStatBtn_OnClick(object sender, RoutedEventArgs e){
        DateTime dateTimeNow = DateTime.Now;
        string startDate =  dateTimeNow.ToString("MM-dd-yyyy");
        string endDate = new DateTime(dateTimeNow.Year - 1, dateTimeNow.Month , dateTimeNow.Day).ToString("MM-dd-yyyy");
        
        LoadAllStatistic(startDate, endDate);
    }

    private void AllTimeAllStatBtn_OnClick(object sender, RoutedEventArgs e){
        string startDate = DateTime.MinValue.ToString("MM-dd-yyyy");
        string endDate = DateTime.MaxValue.ToString("MM-dd-yyyy");
        
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
}