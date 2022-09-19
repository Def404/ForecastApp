using System;
using System.Collections.Generic;
using System.Windows;
using Npgsql;

namespace ForecastApp.categories;

public class DbModuleCategory{

   //private List<Category> _categories = new List<Category>(); 

    public List<Category> GetCategoryList(){
        
        List<Category> _categories = new List<Category>();
        
       // if (_categories.Count >= 1) return _categories;
        
        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();
            
        var sqlCommand = "SELECT * FROM categories WHERE user_login = 'adef_test'";
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
            
        sqlConnector.OpenConnection();

        try{
            NpgsqlDataReader reader = command.ExecuteReader();
            
            var index = 1;

            while (reader.Read()){
                _categories.Add(new Category(index++,reader["category_name"].ToString()));
            }
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
        
        return _categories;

    }

    public void SetCategory(string categoryName){

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand = $"INSERT INTO categories(category_name, user_login) VALUES ('{categoryName}', 'adef_test')";
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        
        sqlConnector.OpenConnection();

        try{
            command.ExecuteNonQuery();
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
    }

    public bool CheckExistence(string categoryName){

        Boolean result;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            $"SELECT EXISTS(SELECT category_name FROM categories WHERE category_name = '{categoryName}' AND user_login = 'adef_test')";

        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        
        sqlConnector.OpenConnection();

        try{
            result = (Boolean)command.ExecuteScalar();
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
            result = true;
        }

        return result;
    }
}