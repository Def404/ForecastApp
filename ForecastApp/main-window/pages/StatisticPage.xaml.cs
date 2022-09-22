using System.Collections.Generic;
using System.Windows.Controls;
using ForecastApp.statistic;

namespace ForecastApp.main_window.pages;

public partial class StatisticPage : UserControl{
    private DbModuleStatistic _moduleStatistic = new DbModuleStatistic();
    public StatisticPage(){
        InitializeComponent();
        
        LoadAllStatistic();
    }

    private void LoadAllStatistic(){
        
        List<AllStatistic> alList = _moduleStatistic.GetAllStaList();

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
        SumPriceTxtBlock.Text = sumPrice.ToString()+" ₽";
        CntSumTxtBlock.Text = sumCnt.ToString();
    }
}