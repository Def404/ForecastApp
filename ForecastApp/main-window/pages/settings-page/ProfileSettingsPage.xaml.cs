using System.Windows;
using System.Windows.Controls;
using ForecastApp.database;
using ForecastApp.database.user;

namespace ForecastApp.main_window.pages.settings_page;

public partial class ProfileSettingsPage : UserControl{
    public ProfileSettingsPage(){
        InitializeComponent();
    }

    private void ProfileSettingsPage_OnLoaded(object sender, RoutedEventArgs e){

        User user = new User();
        user.GetUserFromJson();

        NameTxtBlock.Text = user.Name + " " + user.Surname;
        LoginTxtBlock.Text = user.Login;
        EmailTxtBlock.Text = user.Email;
    }
}