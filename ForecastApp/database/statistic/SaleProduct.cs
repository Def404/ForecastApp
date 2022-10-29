using System;

namespace ForecastApp.database.statistic;

public class SaleProduct{
    public DateTime Date { get; }
    public int CntProduct { get; }

    public SaleProduct(string month, string year, int cntProduct){
        Date = Convert.ToDateTime(month + "." + year);
        CntProduct = cntProduct;
    }

    public SaleProduct(DateTime date, int cntProduct){
        Date = date;
        CntProduct = cntProduct;
    }
    
}