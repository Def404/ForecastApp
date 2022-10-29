namespace ForecastApp.database.forecast;

public class ProductCntMonth{
    public string Date{ get; }
    public int Cnt{ get; }

    public ProductCntMonth(string month, string year, int cnt){
        Date = month + "." + year;
        Cnt = cnt;
    }
}