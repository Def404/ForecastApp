using System;
using System.Collections.Generic;
using System.Windows;
using Npgsql;

namespace ForecastApp.sales;

public class DbModuleSale{

    public List<Sale> GetSaleList(){

        List<Sale> sales = new List<Sale>();

        var login = MainWindow._user.Login;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            "SELECT p.product_name, c.category_name, s.cnt_product, to_char(s.sale_date,  'dd.MM.yyyy') as sale_date, s.sale_price FROM sales s, products p, categories c WHERE s.product_id=p.product_id AND p.category_id = c.category_id AND p.user_login = @l";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("l", login);

        sqlConnector.OpenConnection();
        
        try{
            NpgsqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read()){
                sales.Add(new Sale(){
                    
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
        
        return sales;
    }

    public void SetSale(int productID, int cntProduct, string saleDate){
        
        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            "INSERT INTO sales (product_id, cnt_product, sale_date, sale_price) VALUES ((@pId), @cntPr, to_date(@slDate, 'MM-dd-yyyy'), (SELECT price FROM products p WHERE p.product_id=@pId));";

        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("pId", productID);
        command.Parameters.AddWithValue("cntPr", cntProduct);
        command.Parameters.AddWithValue("slDate", saleDate);
        
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