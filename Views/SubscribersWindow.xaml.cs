using System.Windows;
using PressSubscription.Data;
using Microsoft.EntityFrameworkCore;

namespace PressSubscription.Views;

public partial class SubscribersWindow : Window
{
    private readonly AppDbContext _db = new();

    public SubscribersWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        SubscribersGrid.ItemsSource = _db.Subscribers.ToList();
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        var name = Microsoft.VisualBasic.Interaction.InputBox("ФИО:");
        if (string.IsNullOrWhiteSpace(name)) return;

        var email = Microsoft.VisualBasic.Interaction.InputBox("Email:");
        if (string.IsNullOrWhiteSpace(email)) return;

        var sub = new Models.Subscriber
        {
            FullName = name,
            Email = email
        };

        _db.Subscribers.Add(sub);
        _db.SaveChanges();
        LoadData();
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (SubscribersGrid.SelectedItem is Models.Subscriber sub)
        {
            _db.Subscribers.Remove(sub);
            _db.SaveChanges();
            LoadData();
        }
    }
}