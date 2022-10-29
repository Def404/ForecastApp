using System.Text.RegularExpressions;
using System.Windows;
using ForecastApp.database;
using ForecastApp.database.user;
using ForecastApp.main_window;

namespace ForecastApp.registration_window;

public partial class RegistrationWindow : Window{
    
    private readonly DbModuleUser _moduleUser = new DbModuleUser();
    public RegistrationWindow(){
        InitializeComponent();
    }

    private void AuthBtn_OnClick(object sender, RoutedEventArgs e){
        if (NameTxtBox.Text.Trim() == "" ||
            SurnameTxtBox.Text.Trim() == "" ||
            EmailTxtBox.Text.Trim() == "" ||
            LoginTxtBox.Text.Trim() == "" ||
            PasswordBox.Password.Trim() == "" ||
            RepeatPasswordBox.Password.Trim() == ""){

            ErrorCard.Visibility = Visibility.Visible;
            ErrorTxtBLock.Text = "Не все поля заполены!";
            return;
        }
        
        if (LoginTxtBox.Text.Trim().Length < 5){
            ErrorCard.Visibility = Visibility.Visible;
            ErrorTxtBLock.Text = "Логин должен быть больше 5 символов!";
            return;
        }
        
        if (PasswordBox.Password.Trim().Length < 5 || PasswordBox.Password.Trim().Length > 50){
            ErrorCard.Visibility = Visibility.Visible;
            ErrorTxtBLock.Text = "Пароль должен быть больше 5 и меньше 50 символов!";
            return;
        }
        
        if (Regex.IsMatch(LoginTxtBox.Text.Trim(), "[а-яА-Я]") || Regex.IsMatch(PasswordBox.Password.Trim(), "[а-яА-Я]")){
            ErrorCard.Visibility = Visibility.Visible;
            ErrorTxtBLock.Text = "Логин или пароль должен содеражать только латинский алфавит!";
            return;
        }

        if (PasswordBox.Password.Trim() != RepeatPasswordBox.Password.Trim()){
            ErrorCard.Visibility = Visibility.Visible;
            ErrorTxtBLock.Text = "Пароли должны совпадать!";
            return;
        }

        if (_moduleUser.CheckUser(LoginTxtBox.Text.Trim())){
            ErrorCard.Visibility = Visibility.Visible;
            ErrorTxtBLock.Text = "Пользователь уже сущесвует!";
            return;
        }


        User user = new User(LoginTxtBox.Text.Trim(),
            NameTxtBox.Text.Trim(),
            SurnameTxtBox.Text.Trim(),
            EmailTxtBox.Text.Trim(),
            PasswordBox.Password.Trim());
        

        if (_moduleUser.InsertUser(user) == 1){
            
            if (user.SaveUserToJson() != true) return;
            
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        
            Close();
        }
        else{
            ErrorCard.Visibility = Visibility.Visible;
            ErrorTxtBLock.Text = "Ошибка регистрации";
        }
    }
}