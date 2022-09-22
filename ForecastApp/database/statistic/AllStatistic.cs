namespace ForecastApp.statistic;

public class AllStatistic{
    public string ProductName{ get; }
    public int SumCnt{ get;}
    public decimal SumPrice{ get; }

    public AllStatistic(string productName, int sumCnt, decimal sumPrice){
        ProductName = productName;
        SumCnt = sumCnt;
        SumPrice = sumPrice;
    }
}