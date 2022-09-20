using System;
using System.Collections.Generic;
using System.Windows;
using Npgsql;

namespace ForecastApp.products;

public class DbModuleProduct{

    public List<Product> GetProductList(){

        List<Product> _products = new List<Product>();

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            "SELECT p.product_name, c.category_name, p.price FROM products p, categories c  WHERE  p.category_id = c.category_id AND p.user_login = 'adef_test'";
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());

        sqlConnector.OpenConnection();

        try{
            NpgsqlDataReader reader = command.ExecuteReader();

            var index = 1;

            while (reader.Read()){
                _products.Add(new Product(
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
        
        return _products;
    }

    public void SetProduct(string productName, string category, decimal price){

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            $"INSERT INTO products(product_name, category_id, price, user_login) VALUES ('{productName}', (SELECT category_id FROM categories WHERE categories.category_name = '{category}' AND categories.user_login = 'adef_test'), {price}, 'adef_test')";

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

    public bool CheckExistence(string productName, string categoryName){
        bool result;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            $"SELECT  EXISTS(SELECT * FROM products p WHERE p.product_name = '{productName}' AND p.category_id  = (SELECT category_id FROM categories WHERE category_name = '{categoryName}') AND p.user_login = 'adef_test')";

        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        
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
}