using System.Text.RegularExpressions;
using System.Windows;
using ForecastApp.database.user;
using ForecastApp.registration_window;

namespace ForecastApp.authorization_window;

public partial class AuthorizationWindow : Window{
    
    private DbModuleUser _moduleUser = new DbModuleUser();
    public AuthorizationWindow(){
        InitializeComponent();
    }

    private void AuthBtn_OnClick(object sender, RoutedEventArgs e){

        if (LoginTxtBox.Text.Trim() == "" ||
            PasswordBox.Password.Trim() == ""){
            
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
        
        bool checkAuth = _moduleUser.CheckUserWithPass(LoginTxtBox.Text.Trim(), PasswordBox.Password.Trim());

        if (checkAuth == true){
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        
            Close();
        }
        else{
            ErrorCard.Visibility = Visibility.Visible;
            ErrorTxtBLock.Text = "Ошибка авторизации";
            return;
        }
    }

    private void RegBtn_OnClick(object sender, RoutedEventArgs e){

        RegistrationWindow registrationWindow = new RegistrationWindow();
        registrationWindow.Show();

        Close();
    }
}