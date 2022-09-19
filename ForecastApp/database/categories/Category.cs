namespace ForecastApp.categories;

public class Category{
    
    public int Index{ get; }
    public string Name{ get; }
    

    public Category(int index, string name){
        Index = index;
        Name = name;
    }
}