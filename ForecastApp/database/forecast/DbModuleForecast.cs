using System;
using System.Collections.Generic;
using System.Windows;
using ForecastApp;
using Npgsql;


public class DbModuleForecast{

    public List<ProductCntMonth> GetProductCntMonths(string product, string category){

        List<ProductCntMonth> productCntMonths = new List<ProductCntMonth>();

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand = $"SELECT extract(month from sale_date) AS month, extract(year from sale_date) AS year, sum(cnt_product) AS sumCnt FROM sales s WHERE s.sale_date >  NOW() - '10 MONTH'::INTERVAL AND s.product_id = (SELECT p.product_id FROM products p WHERE p.product_name ='{product}' AND p.category_id = (SELECT category_id FROM categories c WHERE c.category_name = '{category}' AND c.user_login='adef_test')) GROUP BY month, year ORDER BY year DESC, month DESC LIMIT 10;";
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        
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