using System;
using System.Collections.Generic;
using System.Windows;

namespace ForecastApp.main_window.pages;

public class ForecastingMethod{
    public int ExtrapolationMethod(List<ProductCntMonth> productCntMonthList){
        
        var firstDate =  DateTime.Now;
        var endDate = Convert.ToDateTime(productCntMonthList[^1].Date);
        var monthNumber = Math.Abs(firstDate.Month - endDate.Month + 12 * (firstDate.Year - endDate.Year) + 1);
        
        int forecastValue = 0;
        
        if (monthNumber < 3){
            return forecastValue = -1;
        }
        
        foreach (var productCntMonth in productCntMonthList){
            forecastValue += productCntMonth.Cnt;
        }

        forecastValue /= monthNumber;

        return forecastValue;
    }

    public int RegressionMethod(List<ProductCntMonth> productCntMonthList){

        int forecastValue = 0;
        
        var firstDate =  DateTime.Now;
        var endDate = Convert.ToDateTime(productCntMonthList[^1].Date);
        var monthNumber = Math.Abs(firstDate.Month - endDate.Month + 12 *(firstDate.Year - endDate.Year) + 2);
        
      
        if (monthNumber < 15){
            return forecastValue = -1;
        }
        
        int sumCntProduct = 0;
        int sumNumMonth = 0;
        int sumСompositionProductMonth = 0;
        int sumSquareNumMonth = 0;

        for (DateTime date = endDate; date < firstDate.AddMonths(1); date = date.AddMonths(1)){
          
            sumNumMonth += date.Month;
            sumSquareNumMonth += (int)Math.Pow(date.Month, 2);
        }

        foreach (var productCntMonth in productCntMonthList){
            sumCntProduct += productCntMonth.Cnt;
            sumSquareNumMonth += Convert.ToDateTime(productCntMonth.Date).Month * productCntMonth.Cnt;
        }

        double a1 = monthNumber * sumSquareNumMonth - Math.Pow(sumNumMonth, 2);
        double a = (monthNumber * sumСompositionProductMonth - sumNumMonth * sumCntProduct) / a1;
        double b = (sumCntProduct - a * sumNumMonth) / monthNumber;

        forecastValue = (int) Math.Round(a * firstDate.AddMonths(1).Month + b);
        
        return forecastValue;
    }
    
}