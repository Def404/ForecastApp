using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace ForecastApp.database;

public class User{
    
    private static readonly string AppDate = 
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ForecastApp");

    private static readonly string FileName = "user_info.json";
    
    public string? Login{ get; set; }
    public string? Name{ get; set; }
    public string? Surname{ get; set; }
    public string? Email{ get; set; }
    [JsonIgnore]
    public string? HashPassword{ get; }

    public User(){
        
    }
    public User(string? login, string? name, string? surname, string? email, string? password){
        Login = login;
        Name = name;
        Surname = surname;
        Email = email;
        HashPassword = password;
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

    public bool SaveUserToJson(){
        
        try{
            if (!Directory.Exists(AppDate)){
                DirectoryInfo di = Directory.CreateDirectory(AppDate);
            }

            var path = Path.Combine(AppDate, FileName);
            if (File.Exists(path)){
                File.Delete(path);
            }
            
            using FileStream fileStream = File.Create(path);

            User user = this;
            JsonSerializer.Serialize(fileStream, user);
            fileStream.Close();

            return true;
        }
        catch (Exception e){
            MessageBox.Show(e.Message);
            return false;
        }

    }

    public void GetUserFromJson(){
        try{
            
            if (!Directory.Exists(AppDate)){
               return;
            }
            
            string json = File.ReadAllText(Path.Combine(AppDate, FileName));
            User? user = JsonSerializer.Deserialize<User>(json);
            
            if (user == null)
                return;
            
            this.Login = user.Login;
            this.Name = user.Name;
            this.Surname = user.Surname;
            this.Email = user.Email;

        }
        catch (Exception e){
            MessageBox.Show(e.Message);
        }
    }
}