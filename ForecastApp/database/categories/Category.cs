namespace ForecastApp.database.categories;

public class Category{
    
    public int Index{ get; }
    public int Id{ get; }
    public string Name{ get; }
    

    public Category(int index,int id, string name){
        Index = index;
        Id = id;
        Name = name;
    }
}