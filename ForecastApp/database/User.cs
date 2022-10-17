using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ForecastApp;

public class User{
    private string Login;
    private string Name;
    private string Surname;
    private string Email;
    [JsonIgnore]
    private string HashPassword;

    public User(string login, string name, string surname, string email, string password){
        Login = login;
        Name = name;
        Surname = surname;
        Email = email;
        HashPassword = GetHashPassword(password);
    }

    private static string GetHashPassword(string password){

        byte[] salt;
        byte[] interPassword;
        byte[] hashPassword = new byte[0x31]; 
        
        using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8)){
            salt = bytes.Salt;
            interPassword = bytes.GetBytes(0x20);
        }
        
        Buffer.BlockCopy(salt, 0,hashPassword,1,0x10);
        Buffer.BlockCopy(interPassword, 0, hashPassword,0x11,0x20);

        return Convert.ToBase64String(hashPassword);
    }

    public bool CheckHashPassword(string password){

        byte[] hashPassword = Convert.FromBase64String(HashPassword);

        byte[] salt = new byte[0x10];
        byte[] interHashPassword = new byte[0x20];

        byte[] interPassword; 
        
        Buffer.BlockCopy(hashPassword, 1, salt, 0, 0x10);

       
        Buffer.BlockCopy(hashPassword,0x11,interHashPassword,0,0x20);

        using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, salt, 0x3e8)){
            interPassword = bytes.GetBytes(0x20);
        }

        return ByteArrayEqual(interHashPassword, interPassword);
    }

    public void SaveUserToJson(){

        string appDate = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ForecastApp");

        string json = JsonSerializer.Serialize(this);
        File.WriteAllText(Path.Combine(appDate, "user_info.json"), json);
    }

    private static bool ByteArrayEqual(byte[] bytes1, byte[] bytes2){
        IStructuralEquatable structuralEquatable = bytes1;
        return structuralEquatable.Equals(bytes2, StructuralComparisons.StructuralEqualityComparer);
    }
}