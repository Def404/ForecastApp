using System;
using System.Collections.Generic;
using System.Windows;
using Npgsql;

namespace ForecastApp.statistic;

public class DbModuleStatistic{

    public List<AllStatistic> GetAllStatList(string startDate, string endDate){

        List<AllStatistic> allStatistics = new List<AllStatistic>();

        startDate += " 00:00";
        endDate += " 23:59";

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand =
            $"SELECT p.product_name,  sum(cnt_product) as sum_cnt, sum(sale_price*cnt_product) as sum_price FROM sales s, products p WHERE sale_date >= '{startDate}' AND sale_date<='{endDate}' AND s.product_id = p.product_id  GROUP BY s.product_id, p.product_name";

        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        
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
    
    
}