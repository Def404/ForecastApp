using System;
using System.Collections.Generic;
using System.Windows;
using Npgsql;

namespace ForecastApp.sales;

public class DbModuleSale{

    public List<Sale> GetSaleList(){

        List<Sale> _sales = new List<Sale>();

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            "SELECT p.product_name, c.category_name, s.cnt_product, s.sale_date, s.sale_price FROM sales s, products p, categories c WHERE s.product_id=p.product_id AND p.category_id = c.category_id AND p.user_login = 'adef_test'";
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());

        sqlConnector.OpenConnection();
        
        try{
            NpgsqlDataReader reader = command.ExecuteReader();
            var index = 1;

            while (reader.Read()){
                _sales.Add(new Sale(){
                    Index = index++,
                    ProductName = reader["product_name"].ToString(),
                    CategoryName = reader["category_name"].ToString(),
                    CntProduct = Convert.ToInt32(reader["cnt_product"]),
                    SaleData = Convert.ToDateTime(reader["sale_date"]),
                    SalePrice = Convert.ToDecimal(reader["sale_price"])
                });
            }
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
        
        return _sales;
    }
}