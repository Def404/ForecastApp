using System;
using System.Collections.Generic;
using System.Windows;
using Npgsql;
using NpgsqlTypes;

namespace ForecastApp.statistic;

public class DbModuleStatistic{

    public List<AllStatistic> GetAllStatList(string startDate, string endDate){

        List<AllStatistic> allStatistics = new List<AllStatistic>();

        startDate += " 00:00";
        endDate += " 23:59";
        
        var login = MainWindow._user.Login;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            "SELECT p.product_name,  sum(cnt_product) as sum_cnt, sum(sale_price*cnt_product) as sum_price FROM sales s, products p WHERE sale_date >= @startDate AND sale_date<=@endDate AND s.product_id = p.product_id AND p.user_login = @l GROUP BY s.product_id, p.product_name";

        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("startDate", NpgsqlDbType.Date, Convert.ToDateTime(startDate));
        command.Parameters.AddWithValue("endDate", NpgsqlDbType.Date, Convert.ToDateTime(endDate));
        command.Parameters.AddWithValue("l", login);
        
        sqlConnector.OpenConnection();

        try{
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read()){
                allStatistics.Add(new AllStatistic(
                    reader["product_name"].ToString(),
                    Convert.ToInt32(reader["sum_cnt"]),
                    Convert.ToDecimal(reader["sum_price"])));
            }
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
        
        return allStatistics;
    }

    public List<BestSaleProduct> GetBestCntSaleProductList(){

        List<BestSaleProduct> bestSaleProducts = new List<BestSaleProduct>();

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();
        
        var login = MainWindow._user.Login;
        
          var sqlCommand =
              "CREATE TEMP TABLE tempBestCnt AS SELECT p.product_id, p.product_name, c.category_name, extract(month from s.sale_date) AS month, extract(year from s.sale_date) AS year, sum(s.cnt_product) AS sumCnt, sum(s.sale_price * s.cnt_product) as sumPrice FROM sales s JOIN products p on s.product_id = p.product_id JOIN categories c on c.category_id = p.category_id WHERE p.user_login=@l GROUP BY p.product_id, month,year, p.product_name, c.category_name; SELECT product_id, product_name, category_name, year, month, sumCnt, sumPrice FROM tempBestCnt WHERE (sumCnt, year, month) IN (SELECT max(sumCnt), year, month FROM tempBestCnt GROUP BY year, month) ORDER BY year DESC, month DESC;";
          
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("l", login);
        
        sqlConnector.OpenConnection();

        try{
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read()){
                bestSaleProducts.Add(new BestSaleProduct(
                    reader["product_name"].ToString(),
                    reader["category_name"].ToString(),
                    Convert.ToInt32(reader["year"]),
                    Convert.ToInt32(reader["month"]),
                    Convert.ToInt32(reader["sumCnt"]),
                    Convert.ToDecimal(reader["sumPrice"])
                ));
            }
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
        
        return bestSaleProducts;
    }
    
    public List<BestSaleProduct> GetBestPriceSaleProductList(){

        List<BestSaleProduct> bestSaleProducts = new List<BestSaleProduct>();
        
        var login = MainWindow._user.Login;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();
        
          var sqlCommand =
              "CREATE TEMP TABLE tempBestPrice AS SELECT p.product_id, p.product_name, c.category_name, extract(month from s.sale_date) AS month, extract(year from s.sale_date) AS year, sum(s.cnt_product) AS sumCnt, sum(s.sale_price * s.cnt_product) as sumPrice FROM sales s JOIN products p on s.product_id = p.product_id JOIN categories c on c.category_id = p.category_id WHERE p.user_login=@l GROUP BY p.product_id, month,year, p.product_name, c.category_name; SELECT product_id, product_name, category_name, year, month, sumCnt, sumPrice FROM tempBestPrice WHERE (sumPrice, year, month) IN (SELECT max(sumPrice), year, month FROM tempBestPrice GROUP BY year, month) ORDER BY year DESC, month DESC;";
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("l", login);
        
        sqlConnector.OpenConnection();

        try{
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read()){
                bestSaleProducts.Add(new BestSaleProduct(
                    reader["product_name"].ToString(),
                    reader["category_name"].ToString(),
                    Convert.ToInt32(reader["year"]),
                    Convert.ToInt32(reader["month"]),
                    Convert.ToInt32(reader["sumCnt"]),
                    Convert.ToDecimal(reader["sumPrice"])
                ));
            }
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
        
        return bestSaleProducts;
    }

    public List<SaleProduct> GetSaleProducts(int productId){

        List<SaleProduct> saleProducts = new List<SaleProduct>();
        
        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            "SELECT extract(month from sale_date) AS month, extract(year from sale_date) AS year,sum(cnt_product) AS sumCnt FROM sales s WHERE s.product_id = @p GROUP BY month, year ORDER BY year DESC, month DESC;";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        command.Parameters.AddWithValue("p", productId);

        sqlConnector.OpenConnection();

        try{
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read()){
                saleProducts.Add(new SaleProduct(
                    reader["month"].ToString(),
                    reader["year"].ToString(),
                    Convert.ToInt32(reader["sumcnt"]))
                );
            }
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();
        
        return saleProducts;
    }
}