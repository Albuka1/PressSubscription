using System.Windows;
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

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        AuthService.Logout();
        
        var loginWindow = new LoginWindow();
        loginWindow.Show();
        Close();
    }
}