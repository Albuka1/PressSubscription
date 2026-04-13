using System.Windows;
using Microsoft.EntityFrameworkCore;
using PressSubscription.Data;

namespace PressSubscription.Views;

public partial class SubscriptionsWindow : Window
{
    private readonly AppDbContext _db = new();

    public SubscriptionsWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        SubscriptionsGrid.ItemsSource = _db.Subscriptions
            .Include(x => x.Subscriber)
            .Include(x => x.Publication)
            .ToList();
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        var sidText = Microsoft.VisualBasic.Interaction.InputBox("ID подписчика:");
        if (!int.TryParse(sidText, out var sid)) return;

        var pidText = Microsoft.VisualBasic.Interaction.InputBox("ID издания:");
        if (!int.TryParse(pidText, out var pid)) return;

        var subscription = new Models.Subscription
        {
            SubscriberId = sid,
            PublicationId = pid,
            StartDate = DateTime.Now
        };

        _db.Subscriptions.Add(subscription);
        _db.SaveChanges();
    LoadData();
    }
}