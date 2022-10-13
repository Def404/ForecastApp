using System;
using System.Collections.Generic;
using System.Windows;

namespace ForecastApp.main_window.pages;

public class ForecastingMethod{
    public int ExtrapolationMethod(List<ProductCntMonth> productCntMonthList){
        var firstDate =  DateTime.Now;
        var endDate = Convert.ToDateTime(productCntMonthList[^1].Date);
        var monthNumber = Math.Abs(firstDate.Month - endDate.Month + (firstDate.Year - endDate.Year) +1);

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

}