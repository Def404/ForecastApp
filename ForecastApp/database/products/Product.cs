namespace ForecastApp.products;

public class Product{
    
    public int Index{ get; }
    public string Name{ get; }
    public string Category{ get; }
    public decimal Price{ get; }

    public Product(int index, string name, string category, decimal price){
        Index = index;
        Name = name;
        Category = category;
        Price = price;
    }
}