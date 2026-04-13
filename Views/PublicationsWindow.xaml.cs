using System.Linq;
using System.IO;
using System.Windows;
using PressSubscription.Data;

namespace PressSubscription.Views;

public partial class PublicationsWindow : Window
{
    private readonly AppDbContext _db = new();

    public PublicationsWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        PublicationsList.ItemsSource = _db.Publications.ToList();
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        var title = Microsoft.VisualBasic.Interaction.InputBox("Название:");
        if (string.IsNullOrWhiteSpace(title)) return;

        var publisher = Microsoft.VisualBasic.Interaction.InputBox("Издатель:");
        if (string.IsNullOrWhiteSpace(publisher)) return;

        var priceText = Microsoft.VisualBasic.Interaction.InputBox("Цена:");
        if (!decimal.TryParse(priceText, out var price)) return;

        var imageFile = Microsoft.VisualBasic.Interaction.InputBox("Имя картинки (news.png):");
        if (string.IsNullOrWhiteSpace(imageFile))
            imageFile = "placeholder.png";

        var imagePath = Path.Combine("Images", imageFile);

        var pub = new Models.Publication
        {
            Title = title,
            Publisher = publisher,
            PricePerMonth = price,
            ImagePath = imagePath
        };

        _db.Publications.Add(pub);
        _db.SaveChanges();

        LoadData();
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (PublicationsList.SelectedItem is Models.Publication pub)
        {
            _db.Publications.Remove(pub);
            _db.SaveChanges();
            LoadData();
        }
    }
}