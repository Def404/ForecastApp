using System.Windows;
using Npgsql;

namespace ForecastApp.database;

public class PostgreSqlConnector{

    private const string Host = "localhost";
    private const string Port = "5432";
    private const string User = "adef";
    private const string Password = "199as55";
    private const string Database = "forecast";

    private const string _connectionStr = 
        $"Server={Host};Port={Port};User ID={User};Password={Password};Database={Database}";

    private readonly NpgsqlConnection _connection = new NpgsqlConnection(_connectionStr);

    public NpgsqlConnection GetConnection(){
        return _connection;
    }

    public void OpenConnection(){
        try{
            _connection.Open();
        }
        catch (NpgsqlException e){
            MessageBox.Show(e.Message);
        }
    }
    
    public void CloseConnection(){
        try{
            _connection.Close();
        }
        catch (NpgsqlException e){
            MessageBox.Show(e.Message);
        }
    }
}