using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ForecastApp.categories;
using ForecastApp.products;

namespace ForecastApp.main_window.pages;

public partial class CategorySettingsPage : UserControl{

    private readonly DbModuleCategory _moduleCategory = new DbModuleCategory();
    private readonly DbModuleProduct _moduleProduct = new DbModuleProduct();
    
    public CategorySettingsPage(){
        InitializeComponent();
    }

    private void CategorySettingsPage_OnLoaded(object sender, RoutedEventArgs e){
        
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
        if (DelCategoryListCmbBox.Text.Length <= 0){
            MessageBox.Show("Укажите название категории");
            return;
        }

        var category = (Category)DelCategoryListCmbBox.SelectedItem;
        if (_moduleProduct.GetProductsOfCatList(category.Id).Count > 0){
            MessageBox.Show("У категории есть товары");
            return;
        }
        
        _moduleCategory.DelCategory(category.Name);
        DelCategoryListCmbBox.SelectedItem = -1;
        
        UpdateForm();
    }

    private void UpdateCategoryBtn_OnClick(object sender, RoutedEventArgs e){
        if (ChangeCategoryListCmbBox.SelectedIndex == -1 ||
            ChangeCategoryNameTxtBox.Text.Trim() == ""){
            MessageBox.Show("Укажите название категории");
            return;
        }
        
        var category = (Category)ChangeCategoryListCmbBox.SelectedItem;
        
        _moduleCategory.UpdateCategory(category.Name, ChangeCategoryNameTxtBox.Text.Trim());
        ChangeCategoryListCmbBox.SelectedIndex = -1;
        ChangeCategoryNameTxtBox.Clear();
        
        UpdateForm();
    }

    private void UpdateForm(){
        
        var categories = _moduleCategory.GetCategoryList();
        CategoryListDataGrid.ItemsSource = categories;
        DelCategoryListCmbBox.ItemsSource = categories;
        ChangeCategoryListCmbBox.ItemsSource = categories;
    }
}