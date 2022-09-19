using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ForecastApp.categories;

namespace ForecastApp.main_window.pages;

public partial class CategorySettingsPage : UserControl{

    private readonly DbModuleCategory _moduleCategory = new DbModuleCategory();

    //private List<Category> _categories = new List<Category>();
    public CategorySettingsPage(){
        InitializeComponent();
    }

    private void CategorySettingsPage_OnLoaded(object sender, RoutedEventArgs e){
    
        /*
        if (_categories.Count <= 0){
            _categories = _moduleCategory.GetCategoryList();
        }*/

        var categories = _moduleCategory.GetCategoryList();
        
        CategoryListDataGrid.ItemsSource = categories;
        DelCategoryListCmbBox.ItemsSource = categories;
        ChangeCategoryListCmbBox.ItemsSource = categories;
    }

    private void AddCategoryBtn_OnClick(object sender, RoutedEventArgs e){

        string newCategoryName = NewCategoryNameTxtBox.Text.Trim();
        
        if (newCategoryName.Length <= 0){
            MessageBox.Show("Укажите название категории");
            return;
        }

        if (_moduleCategory.CheckExistence(newCategoryName)){
            MessageBox.Show("Данная категория уже сущесвует");
            return;
        }
        
        _moduleCategory.SetCategory(newCategoryName);
        NewCategoryNameTxtBox.Clear();
        UpdateForm();
    }

    private void DelCategoryBtn_OnClick(object sender, RoutedEventArgs e){
        
    }

    private void UpdateCategoryBtn_OnClick(object sender, RoutedEventArgs e){
        
    }

    private void UpdateForm(){
        
        var categories = _moduleCategory.GetCategoryList();
        CategoryListDataGrid.ItemsSource = categories;
        DelCategoryListCmbBox.ItemsSource = categories;
        ChangeCategoryListCmbBox.ItemsSource = categories;
    }
}