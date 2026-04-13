using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace PressSubscription.Models;

public class Publication
{
    public int Id { get; set; }

    public string Title { get; set; } = "";
    public string Publisher { get; set; } = "";
    public decimal PricePerMonth { get; set; }
    public string ImagePath { get; set; } = "";

    public BitmapImage Image
    {
        get
        {
            var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImagePath);

            var finalPath = File.Exists(fullPath)
                ? fullPath
                : GetPlaceholderPath();

            return new BitmapImage(new Uri(finalPath, UriKind.Absolute));
        }
    }

    private static string GetPlaceholderPath()
    {
        return Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Images",
            "placeholder.png");
    }
}