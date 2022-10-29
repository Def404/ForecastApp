using System;
using System.Collections.Generic;
using System.Windows;
using ForecastApp.main_window;
using Npgsql;

namespace ForecastApp.database.forecast;

public class DbModuleForecast{

    public List<ProductCntMonth> GetProductCntMonths(string productName, int categoryId){

        List<ProductCntMonth> productCntMonths = new List<ProductCntMonth>();
        
        var login = MainWindow._user.Login;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        const string sqlCommand = 
            "SELECT extract(month FROM s.sale_date) AS month, extract(year FROM s.sale_date) AS year, sum(s.cnt_product) AS sumCntProd FROM sales s JOIN products p ON p.product_id = s.product_id WHERE p.product_name=@p AND p.category_id=@c AND s.sale_date > NOW() - '12 MONTH'::INTERVAL AND p.user_login=@l GROUP BY month, year ORDER BY year DESC, month DESC LIMIT 12;";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("p", productName);
        command.Parameters.AddWithValue("c", categoryId);
        command.Parameters.AddWithValue("l", login);
        
        sqlConnector.OpenConnection();

        try{
            NpgsqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read()){
                productCntMonths.Add(new ProductCntMonth(
                    reader["month"].ToString(),
                    reader["year"].ToString(),
                    Convert.ToInt32(reader["sumCntProd"])));
            }
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
        
        return productCntMonths;
    }
    
    public List<ProductCntMonth> GetProductCntAllTime(string productName, int categoryId){

        List<ProductCntMonth> productCntMonths = new List<ProductCntMonth>();
        
        var login = MainWindow._user.Login;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        const string sqlCommand = 
            "SELECT extract(month FROM s.sale_date) AS month, extract(year FROM s.sale_date) AS year, sum(s.cnt_product) AS sumCntProd FROM sales s JOIN products p ON p.product_id = s.product_id WHERE p.product_name=@p AND p.category_id=@c AND p.user_login=@l GROUP BY month, year ORDER BY year DESC, month DESC;";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("p", productName);
        command.Parameters.AddWithValue("c", categoryId);
        command.Parameters.AddWithValue("l", login);
        
        sqlConnector.OpenConnection();

        try{
            NpgsqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read()){
                productCntMonths.Add(new ProductCntMonth(
                    reader["month"].ToString(),
                    reader["year"].ToString(),
                    Convert.ToInt32(reader["sumCntProd"])));
            }
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
        
        return productCntMonths;
    }
}