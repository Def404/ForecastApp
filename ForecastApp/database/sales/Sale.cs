using System;

namespace ForecastApp.sales;

public class Sale{
    public string ProductName{ get; set; }
    public string CategoryName{ get; set; }
    public int CntProduct{ get; set; }
    public string SaleData{ get; set; }
    public decimal SalePrice{ get; set; }

    public Sale(){
        
    }
}