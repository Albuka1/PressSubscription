using System;
using PressSubscription.Models;
using PressSubscription.Data;
using Microsoft.EntityFrameworkCore;

namespace PressSubscription.Services;

public static class SubscriptionCalculator
{
    public static void Calculate(Subscription s)
    {
        if (s == null) throw new ArgumentNullException(nameof(s));

        decimal pricePerMonth;

        if (s.Publication != null)
        {
            pricePerMonth = s.Publication.PricePerMonth;
        }
        else
        {
            // Загружаем издание отдельно, без отслеживания
            using var db = new AppDbContext();
            var publication = db.Publications.AsNoTracking().FirstOrDefault(p => p.Id == s.PublicationId);
            if (publication == null)
                throw new InvalidOperationException($"Издание с ID {s.PublicationId} не найдено");
            pricePerMonth = publication.PricePerMonth;
        }

        s.BaseCost = pricePerMonth * s.Months;
        s.DeliveryCost = s.BaseCost * 0.01m;
        s.Vat = s.BaseCost * 0.18m;
        s.TotalCost = s.BaseCost + s.DeliveryCost + s.Vat;
    }
}