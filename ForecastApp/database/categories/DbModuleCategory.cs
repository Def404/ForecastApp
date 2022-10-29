using System;
using System.Collections.Generic;
using System.Windows;
using Npgsql;

namespace ForecastApp.categories;

public class DbModuleCategory{
    
    public List<Category> GetCategoryList(){
        
        List<Category> _categories = new List<Category>();

        var login = MainWindow._user.Login;
        
        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();
            
        var sqlCommand =
            "SELECT * FROM categories WHERE user_login = @l";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("l", login);
            
        sqlConnector.OpenConnection();

        try{
            NpgsqlDataReader reader = command.ExecuteReader();
            
            var index = 1;

            while (reader.Read()){
                _categories.Add(new Category(index++,
                    Convert.ToInt32(reader["category_id"]), 
                    reader["category_name"].ToString()));
            }
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
        
        return _categories;

    }

    public void SetCategory(string categoryName){
        
        var login = MainWindow._user.Login;
        
        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();
        
        var sqlCommand = 
            "INSERT INTO categories(category_name, user_login) VALUES (@c, (SELECT login FROM users WHERE login = @l))";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("c", categoryName);
        command.Parameters.AddWithValue("l", login);
        
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

        bool result;

        var login = MainWindow._user.Login;
        
        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();
        
        var sqlCommand =
            "SELECT EXISTS(SELECT category_name FROM categories WHERE category_name = @c AND user_login = @l)";

        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("c", categoryName);
        command.Parameters.AddWithValue("l", login);
        
        sqlConnector.OpenConnection();

        try{
            result = (bool)command.ExecuteScalar();
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
            result = true;
        }
        
        sqlConnector.CloseConnection();
        
        return result;
    }

    public void DelCategory(string categoryName){

        var login = MainWindow._user.Login;
        
        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand = 
            "DELETE FROM categories WHERE category_name = @c AND user_login=@l";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("c", categoryName);
        command.Parameters.AddWithValue("l", login);
        
        sqlConnector.OpenConnection();

        try{
            command.ExecuteNonQuery();
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
    }

    public void UpdateCategory(string oldCategoryName, string newCategoryName){
        
        var login = MainWindow._user.Login;
        
        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand = 
            "UPDATE categories SET category_name = @newCat WHERE category_name = @oldCat AND user_login = @l";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("newCat", newCategoryName);
        command.Parameters.AddWithValue("oldCat", oldCategoryName);
        command.Parameters.AddWithValue("l", login);
        
        sqlConnector.OpenConnection();

        try{
            command.ExecuteNonQuery();
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
    }
}