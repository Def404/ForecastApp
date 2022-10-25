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
            "CREATE TEMP TABLE tempBestCnt AS SELECT product_id, (SELECT product_name FROM products p WHERE p.product_id = s.product_id) as product_name, (SELECT category_name FROM categories c, products p WHERE p.product_id = s.product_id AND p.category_id = c.category_id AND c.user_login=@l) AS category_name, extract(month from sale_date) AS month, extract(year from sale_date) AS year, sum(cnt_product) AS sumCnt, sum(sale_price * cnt_product) as sumPrice  FROM sales s GROUP BY product_id, month, year; SELECT product_id, product_name, category_name, year, month, sumCnt, sumPrice FROM tempBestCnt WHERE (sumCnt, year, month) IN (SELECT max(sumCnt), year, month FROM tempBestCnt GROUP BY year, month);";

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
            "CREATE TEMP TABLE tempBestPrice AS SELECT product_id, (SELECT product_name FROM products p WHERE p.product_id = s.product_id) as product_name, (SELECT category_name FROM categories c, products p WHERE p.product_id = s.product_id AND p.category_id = c.category_id AND c.user_login=@l) AS category_name, extract(month from sale_date) AS month, extract(year from sale_date) AS year, sum(cnt_product) AS sumCnt, sum(sale_price * cnt_product) as sumPrice  FROM sales s GROUP BY product_id, month, year; SELECT product_id, product_name, category_name, year, month, sumCnt, sumPrice FROM tempBestPrice WHERE (sumPrice, year, month) IN (SELECT max(sumPrice), year, month FROM tempBestPrice GROUP BY year, month);";

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
}