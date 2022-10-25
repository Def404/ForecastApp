using System;
using System.Collections.Generic;
using System.Windows;
using Npgsql;

namespace ForecastApp.products;

public class DbModuleProduct{
    
    public List<Product> GetProductList(){

        List<Product> products = new List<Product>();
        
        var login = MainWindow._user.Login;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            "SELECT p.product_name, c.category_name, p.price FROM products p, categories c  WHERE  p.category_id = c.category_id AND p.user_login = @l";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("l", login);

        sqlConnector.OpenConnection();

        try{
            NpgsqlDataReader reader = command.ExecuteReader();

            var index = 1;

            while (reader.Read()){
                products.Add(new Product(
                    index++,
                    reader["product_name"].ToString(),
                    reader["category_name"].ToString(),
                    (decimal)reader["price"]));
            }
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
        
        return products;
    }

    public void SetProduct(string productName, string category, decimal price){
        
        var login = MainWindow._user.Login;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            "INSERT INTO products(product_name, category_id, price, user_login) VALUES (@p, (SELECT category_id FROM categories WHERE categories.category_name = @c AND categories.user_login = @l), @pr, @l)";

        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("p", productName);
        command.Parameters.AddWithValue("c", category);
        command.Parameters.AddWithValue("pr", price);
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

    public bool CheckExistence(string productName, string category){
        
        bool result;
        
        var login = MainWindow._user.Login;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            "SELECT  EXISTS(SELECT * FROM products p WHERE p.product_name = @p AND p.category_id  = (SELECT category_id FROM categories WHERE category_name = @c) AND p.user_login = @l)";

        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("p", productName);
        command.Parameters.AddWithValue("c", category);
        command.Parameters.AddWithValue("l",login);
        
        sqlConnector.OpenConnection();

        try{
            result = (bool)command.ExecuteScalar();
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
            result = true;
        }
        return result;
    }

    public List<Product> GetProductsOfCatList(string category){
        
        List<Product> products = new List<Product>();
        
        var login = MainWindow._user.Login;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            "SELECT p.product_name, c.category_name, p.price FROM products p, categories c WHERE p.category_id = c.category_id AND c.category_name = @c AND p.user_login = @l";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("c", category);
        command.Parameters.AddWithValue("l", login);

        sqlConnector.OpenConnection();

        try{
            NpgsqlDataReader reader = command.ExecuteReader();

            var index = 1;

            while (reader.Read()){
                products.Add(new Product(
                    index++,
                    reader["product_name"].ToString(),
                    reader["category_name"].ToString(),
                    (decimal)reader["price"]));
            }
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
        
        return products;
    }

    public void DelProduct(string productName, string category){
        
        var login = MainWindow._user.Login;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            "DELETE FROM products WHERE product_name=@p AND category_id = (SELECT category_id FROM categories WHERE category_name=@c AND user_login=@l)";

        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("p", productName);
        command.Parameters.AddWithValue("c", category);
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

    public void UpdateProductName(string oldProductName, string newProductName){
        
        var login = MainWindow._user.Login;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand = 
            "UPDATE products SET product_name = @newPr WHERE product_name = @oldPr AND user_login = @l";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("newPr", newProductName);
        command.Parameters.AddWithValue("oldPr", oldProductName);
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

    public void UpdateProductPrice(string productName, decimal newPrice){
        var login = MainWindow._user.Login;
        
        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand = 
            "UPDATE products SET price = @newPrice WHERE product_name = @p AND user_login = @l";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("newPrice", newPrice);
        command.Parameters.AddWithValue("p", productName);
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