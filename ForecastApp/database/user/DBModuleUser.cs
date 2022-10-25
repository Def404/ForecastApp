using System;
using System.Windows;
using Npgsql;

namespace ForecastApp.database.user;

public class DbModuleUser{

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

    public int InsertUser(User user){
        
        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand = $"INSERT INTO users (login,  user_name, user_surname, email, password) VALUES ('{user.Login}', '{user.Name}', '{user.Surname}', '{user.Email}', crypt('{user.HashPassword}', gen_salt('md5')))";

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

    public bool CheckUserWithPass(string login, string password){
        
        PostgreSqlConnector sqlConnector = new PostgreSqlConnector();

        var sqlCommand = $"SELECT login, user_name, user_surname, email, password FROM  users WHERE login='{login}' AND password = crypt('{password}', password)";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlCommand, sqlConnector.GetConnection());
        
        sqlConnector.OpenConnection();

        User user = null;

        try{
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read()){
                user = new User(reader["login"].ToString(),
                    reader["user_name"].ToString(),
                    reader["user_surname"].ToString(),
                    reader["email"].ToString(),
                    null);
            }

            if (user == null){
                return false;
            }
            
            if (user.SaveUserToJson() != true) return false;

            return true;
        }
        catch (Exception e){
            MessageBox.Show(e.StackTrace);
            return false;
        }
    }
}