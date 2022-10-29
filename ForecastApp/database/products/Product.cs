namespace ForecastApp.database.products;

public class Product{
    
    public int Index{ get; }
    public int Id{ get; }
    public string Name{ get; }
    public string Category{ get; }
    public decimal Price{ get; }

    public Product(int index, int id, string name, string category, decimal price){
        Index = index;
        Id = id;
        Name = name;
        Category = category;
        Price = price;
    }
}