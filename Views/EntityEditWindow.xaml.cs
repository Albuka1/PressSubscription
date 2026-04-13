using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PressSubscription.Models;

namespace PressSubscription.Views;

public partial class EntityEditWindow : Window
{
    public object? Entity { get; private set; }
    private Type? _entityType;
    private readonly Dictionary<string, TextBox> _textBoxes = new();
    private DatePicker? _datePicker;

    public EntityEditWindow()
    {
        InitializeComponent();
    }

    // Для Subscriber
    public void SetEntity(Subscriber? subscriber = null)
    {
        _entityType = typeof(Subscriber);
        
        if (subscriber != null)
        {
            Entity = subscriber;
        }
        else
        {
            Entity = new Subscriber();
        }
        
        BuildFormForSubscriber();
        LoadValues();
    }

    // Для Publication
    public void SetEntity(Publication? publication = null)
    {
        _entityType = typeof(Publication);
        
        if (publication != null)
        {
            Entity = publication;
        }
        else
        {
            Entity = new Publication();
        }
        
        BuildFormForPublication();
        LoadValues();
    }

    // Для Subscription
    public void SetEntity(Subscription? subscription = null)
    {
        _entityType = typeof(Subscription);
        
        if (subscription != null)
        {
            Entity = subscription;
        }
        else
        {
            Entity = new Subscription();
        }
        
        BuildFormForSubscription();
        LoadValues();
    }

    private void BuildFormForSubscriber()
    {
        FieldsPanel.Children.Clear();
        _textBoxes.Clear();
        _datePicker = null;
        
        AddField("FullName", "ФИО:");
        AddField("Address", "Адрес:");
        AddField("Phone", "Телефон:");
        AddField("Email", "Email:");
    }

    private void BuildFormForPublication()
    {
        FieldsPanel.Children.Clear();
        _textBoxes.Clear();
        _datePicker = null;
        
        AddField("Title", "Название:");
        AddField("Publisher", "Издатель:");
        AddField("PricePerMonth", "Цена за месяц (₽):");
        AddField("ImagePath", "Путь к изображению (Images/...):");
    }

    private void BuildFormForSubscription()
    {
        FieldsPanel.Children.Clear();
        _textBoxes.Clear();
        _datePicker = null;
        
        AddField("SubscriberId", "ID подписчика:");
        AddField("PublicationId", "ID издания:");
        AddDateField("StartDate", "Дата начала:");
        AddField("Months", "Количество месяцев:");
    }

    private void AddField(string propertyName, string label)
    {
        var labelBlock = new TextBlock
        {
            Text = label,
            Margin = new Thickness(0, 0, 0, 5),
            FontWeight = FontWeights.SemiBold
        };
        
        var textBox = new TextBox
        {
            Name = $"{propertyName}Box",
            Margin = new Thickness(0, 0, 0, 15),
            Height = 35,
            TextWrapping = TextWrapping.Wrap,
            AcceptsReturn = true,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            MinHeight = 35,
            MaxHeight = 120
        };
        
        FieldsPanel.Children.Add(labelBlock);
        FieldsPanel.Children.Add(textBox);
        
        _textBoxes[propertyName] = textBox;
    }

    private void AddDateField(string propertyName, string label)
    {
        var labelBlock = new TextBlock
        {
            Text = label,
            Margin = new Thickness(0, 0, 0, 5),
            FontWeight = FontWeights.SemiBold
        };
        
        var datePicker = new DatePicker
        {
            Name = $"{propertyName}Picker",
            Margin = new Thickness(0, 0, 0, 15),
            Height = 35,
            SelectedDate = DateTime.Today
        };
        
        FieldsPanel.Children.Add(labelBlock);
        FieldsPanel.Children.Add(datePicker);
        
        _datePicker = datePicker;
    }

    private void LoadValues()
    {
        if (_entityType == null) return;
        
        foreach (var kvp in _textBoxes)
        {
            var property = _entityType.GetProperty(kvp.Key);
            if (property != null)
            {
                var value = property.GetValue(Entity)?.ToString();
                kvp.Value.Text = value ?? "";
            }
        }
        
        if (_datePicker != null && _entityType == typeof(Subscription))
        {
            var property = _entityType.GetProperty("StartDate");
            if (property != null && property.GetValue(Entity) is DateTime date)
            {
                _datePicker.SelectedDate = date;
            }
        }
    }

    private void SaveValues()
    {
        if (_entityType == null) return;
        
        foreach (var kvp in _textBoxes)
        {
            var property = _entityType.GetProperty(kvp.Key);
            if (property != null)
            {
                var value = kvp.Value.Text.Trim();
                
                if (property.PropertyType == typeof(decimal))
                {
                    if (decimal.TryParse(value, out var decimalValue))
                        property.SetValue(Entity, decimalValue);
                }
                else if (property.PropertyType == typeof(int))
                {
                    if (int.TryParse(value, out var intValue))
                        property.SetValue(Entity, intValue);
                }
                else
                {
                    property.SetValue(Entity, value);
                }
            }
        }
        
        if (_datePicker != null && _entityType == typeof(Subscription) && _datePicker.SelectedDate.HasValue)
        {
            var property = _entityType.GetProperty("StartDate");
            if (property != null)
            {
                property.SetValue(Entity, _datePicker.SelectedDate.Value);
            }
        }
    }

    private bool Validate()
    {
        if (_textBoxes.ContainsKey("FullName") && string.IsNullOrWhiteSpace(_textBoxes["FullName"].Text))
        {
            MessageBox.Show("Введите ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
        
        if (_textBoxes.ContainsKey("Title") && string.IsNullOrWhiteSpace(_textBoxes["Title"].Text))
        {
            MessageBox.Show("Введите название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
        
        if (_textBoxes.ContainsKey("PricePerMonth"))
        {
            if (!decimal.TryParse(_textBoxes["PricePerMonth"].Text, out var price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену (больше 0)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }
        
        if (_textBoxes.ContainsKey("SubscriberId"))
        {
            if (!int.TryParse(_textBoxes["SubscriberId"].Text, out var subId) || subId <= 0)
            {
                MessageBox.Show("Введите корректный ID подписчика (больше 0)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }
        
        if (_textBoxes.ContainsKey("PublicationId"))
        {
            if (!int.TryParse(_textBoxes["PublicationId"].Text, out var pubId) || pubId <= 0)
            {
                MessageBox.Show("Введите корректный ID издания (больше 0)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }
        
        if (_textBoxes.ContainsKey("Months"))
        {
            if (!int.TryParse(_textBoxes["Months"].Text, out var months) || months <= 0)
            {
                MessageBox.Show("Введите корректное количество месяцев (больше 0)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }
        
        if (_datePicker != null && !_datePicker.SelectedDate.HasValue)
        {
            MessageBox.Show("Выберите дату начала", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
        
        return true;
    }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
        if (!Validate()) return;
        
        SaveValues();
        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}