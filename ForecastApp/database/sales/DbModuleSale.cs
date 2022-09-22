using System;
using System.Collections.Generic;
using System.Windows;
using ForecastApp.main_window.pages;
using Npgsql;

namespace ForecastApp.sales;

public class DbModuleSale{

    public List<Sale> GetSaleList(){

        List<Sale> _sales = new List<Sale>();

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            "SELECT p.product_name, c.category_name, s.cnt_product, to_char(s.sale_date,  'dd.MM.yyyy') as sale_date, s.sale_price FROM sales s, products p, categories c WHERE s.product_id=p.product_id AND p.category_id = c.category_id AND p.user_login = 'adef_test'";
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());

        sqlConnector.OpenConnection();
        
        try{
            NpgsqlDataReader reader = command.ExecuteReader();
            //var index = 1;

            while (reader.Read()){
                _sales.Add(new Sale(){
                    //Index = index++,
                    ProductName = reader["product_name"].ToString(),
                    CategoryName = reader["category_name"].ToString(),
                    CntProduct = Convert.ToInt32(reader["cnt_product"]),
                    SaleData = reader["sale_date"].ToString(),
                    SalePrice = Convert.ToDecimal(reader["sale_price"]) * Convert.ToInt32(reader["cnt_product"])
                });
            }
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
        
        return _sales;
    }

    public void SetSale(string productName, string categoryName, int cntProduct, string saleDate){

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            $"CREATE TEMP TABLE temp_table AS SELECT product_id, price FROM products WHERE product_name='{productName}' AND products.user_login='adef_test' AND category_id= (SELECT category_id FROM categories WHERE category_name ='{categoryName}' AND categories.user_login='adef_test'); INSERT INTO sales (product_id, cnt_product, sale_date, sale_price) VALUES ((SELECT product_id FROM temp_table), {cntProduct}, to_date('{saleDate}', 'MM-dd-yyyy'), (SELECT price FROM temp_table)); DROP TABLE temp_table;";

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
}