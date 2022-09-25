namespace ForecastApp.statistic;

public class BestSaleProduct{
    public string ProductName { get; }
    public string CategoryName { get; }
    public string Date { get; }
    public int SumCntProduct { get; }
    public decimal SumPrice { get; }

    public BestSaleProduct(string productName, string categoryName, int year, int month, int sumCntProduct,
        decimal sumPrice){
        ProductName = productName;
        CategoryName = categoryName;
        Date = month + "." + year;
        SumCntProduct = sumCntProduct;
        SumPrice = sumPrice;
    }
}