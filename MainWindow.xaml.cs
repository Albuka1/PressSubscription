using System.Windows;
using Microsoft.Win32;
using PressSubscription.Services;
using PressSubscription.Views;

namespace PressSubscription;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        SetUserInfo();
        ApplyAdminVisibility();
        ApplyImportButtonVisibility();
    }

    private void SetUserInfo()
    {
        var user = AuthService.GetCurrentUser();
        if (user != null)
        {
            var roleIcon = user.Role == "Admin" ? "👑" : "👤";
            UserInfoText.Text = $"{roleIcon} {user.FullName}";
        }
    }

    private void ApplyAdminVisibility()
    {
        var isAdmin = AuthService.IsAdmin();
        AdminTabItem.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
    }

    private void ApplyImportButtonVisibility()
    {
        var isAdmin = AuthService.IsAdmin();
        ImportButton.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
    }

    private void Import_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            dialog.Title = "Выберите файл для импорта";

            if (dialog.ShowDialog() == true)
            {
                ExcelImportService.Import(dialog.FileName);
                MessageBox.Show("Импорт данных успешно завершён!", "Успех", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (System.Exception ex)
        {
            MessageBox.Show($"Ошибка при импорте:\n{ex.Message}", "Ошибка", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        AuthService.Logout();
        
        var loginWindow = new LoginWindow();
        loginWindow.Show();
        Close();
    }
}