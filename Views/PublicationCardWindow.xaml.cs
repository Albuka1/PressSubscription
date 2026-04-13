using System;
using System.Linq;
using System.Windows;
using PressSubscription.Data;
using PressSubscription.Models;
using PressSubscription.Services;

namespace PressSubscription.Views;

public partial class PublicationCardWindow : Window
{
    private readonly Publication _publication;
    private readonly AppDbContext _db = new();

    public PublicationCardWindow(Publication publication)
    {
        InitializeComponent();
        _publication = publication;
        LoadPublicationData();
        LoadSubscribers();
        StartDatePicker.SelectedDate = DateTime.Today; // устанавливаем сегодняшнюю дату
        UpdatePreview();
        MonthsBox.TextChanged += (s, e) => UpdatePreview();
        SubscriberCombo.SelectionChanged += (s, e) => UpdatePreview();
    }

    private void LoadPublicationData()
    {
        TitleText.Text = _publication.Title;
        PublisherText.Text = _publication.Publisher;
        PriceText.Text = $"💰 Цена: {_publication.PricePerMonth} ₽/мес";
        PublicationImage.Source = _publication.Image;
    }

    private void LoadSubscribers()
    {
        var subscribers = _db.Subscribers.ToList();
        SubscriberCombo.ItemsSource = subscribers;
        if (subscribers.Any())
            SubscriberCombo.SelectedIndex = 0;
    }

    private void UpdatePreview()
    {
        if (!decimal.TryParse(MonthsBox.Text, out var months) || months <= 0)
        {
            BaseCostPreview.Text = "Базовая стоимость: 0 ₽";
            DeliveryPreview.Text = "Доставка (1%): 0 ₽";
            VatPreview.Text = "НДС (18%): 0 ₽";
            TotalPreview.Text = "Итого: 0 ₽";
            return;
        }

        var baseCost = _publication.PricePerMonth * months;
        var delivery = baseCost * 0.01m;
        var vat = baseCost * 0.18m;
        var total = baseCost + delivery + vat;

        BaseCostPreview.Text = $"Базовая стоимость: {baseCost:N2} ₽";
        DeliveryPreview.Text = $"Доставка (1%): {delivery:N2} ₽";
        VatPreview.Text = $"НДС (18%): {vat:N2} ₽";
        TotalPreview.Text = $"Итого: {total:N2} ₽";
    }

    private void Subscribe_Click(object sender, RoutedEventArgs e)
    {
        if (SubscriberCombo.SelectedItem is not Subscriber subscriber)
        {
            MessageBox.Show("Выберите подписчика", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!int.TryParse(MonthsBox.Text, out var months) || months <= 0)
        {
            MessageBox.Show("Введите корректное количество месяцев (больше 0)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var startDate = StartDatePicker.SelectedDate ?? DateTime.Today;

        var subscription = new Subscription
        {
            SubscriberId = subscriber.Id,
            PublicationId = _publication.Id,
            StartDate = startDate,
            Months = months
        };

        SubscriptionCalculator.Calculate(subscription);

        _db.Subscriptions.Add(subscription);
        _db.SaveChanges();

        MessageBox.Show($"Подписка на \"{_publication.Title}\" оформлена!\n\n" +
                        $"Подписчик: {subscriber.FullName}\n" +
                        $"Период: {startDate:dd.MM.yyyy} на {months} мес.\n" +
                        $"Стоимость: {subscription.TotalCost:N2} ₽",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}