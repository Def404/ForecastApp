using System;
using System.Windows;
using Npgsql;

namespace ForecastApp.database.registration;

public class DBModuleRegistration{

    public bool CheckUser(string userLogin){

        int result = 0;

        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand = $"SELECT CAST(count(*) AS BIT) AS cnt FROM users WHERE login='{userLogin}'";
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        
        sqlConnector.OpenConnection();

        try{
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read()){
                result = Convert.ToInt32(reader["cnt"]);
            }
        }
        catch (Exception e){
            MessageBox.Show(e.StackTrace);
        }

        return (result == 1);
    }

    public int AddUser(User user){
        
        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand = $"INSERT INTO users (login,  user_name, user_surname, email, password) VALUES ('{user.Login}', '{user.Name}', '{user.Surname}', '{user.Email}', '{user.HashPassword}')";

        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        
        sqlConnector.OpenConnection();
        var result = -1;
        try{ 
            result = command.ExecuteNonQuery();
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
        
        sqlConnector.CloseConnection();

        return result;
    }
}