using System;
using System.Collections.Generic;
using System.Windows;
using ForecastApp;
using Npgsql;

public class DbModuleForecast{

    public List<ProductCntMonth> GetProductCntMonths(string productName, string category){

        List<ProductCntMonth> productCntMonths = new List<ProductCntMonth>();
        
        var login = MainWindow._user.Login;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand = 
            "SELECT extract(month from sale_date) AS month, extract(year from sale_date) AS year, sum(cnt_product) AS sumCnt FROM sales s WHERE s.sale_date >  NOW() - '12 MONTH'::INTERVAL AND s.product_id = (SELECT p.product_id FROM products p WHERE p.product_name = @p AND p.category_id = (SELECT category_id FROM categories c WHERE c.category_name = @c AND c.user_login=@l)) GROUP BY month, year ORDER BY year DESC, month DESC LIMIT 12;";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("p", productName);
        command.Parameters.AddWithValue("c", category);
        command.Parameters.AddWithValue("l", login);
        
        sqlConnector.OpenConnection();

        try{
            NpgsqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read()){
                productCntMonths.Add(new ProductCntMonth(
                    reader["month"].ToString(),
                    reader["year"].ToString(),
                    Convert.ToInt32(reader["sumcnt"])));
            }
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
        
        return productCntMonths;
    }
    
    public List<ProductCntMonth> GetProductCntAllTime(string productName, string category){

        List<ProductCntMonth> productCntMonths = new List<ProductCntMonth>();
        
        var login = MainWindow._user.Login;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand = 
            "SELECT extract(month from sale_date) AS month, extract(year from sale_date) AS year, sum(cnt_product) AS sumCnt FROM sales s WHERE s.product_id = (SELECT p.product_id FROM products p WHERE p.product_name = @p AND p.category_id = (SELECT category_id FROM categories c WHERE c.category_name = @c AND c.user_login=@l)) GROUP BY month, year ORDER BY year DESC, month DESC;";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("p", productName);
        command.Parameters.AddWithValue("c", category);
        command.Parameters.AddWithValue("l", login);
        
        sqlConnector.OpenConnection();

        try{
            NpgsqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read()){
                productCntMonths.Add(new ProductCntMonth(
                    reader["month"].ToString(),
                    reader["year"].ToString(),
                    Convert.ToInt32(reader["sumcnt"])));
            }
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
        
        return productCntMonths;
    }
}