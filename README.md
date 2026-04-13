<p align="center">
  <img alt="PressSubscription" width="150" src="/Assets/logo.png" />
</p>

# PressSubscription - Учёт подписки на печатные издания

[![Build .NET 8.0](https://github.com/Albuka1/PressSubscription/actions/workflows/build.yml/badge.svg)](https://github.com/Albuka1/PressSubscription/actions/workflows/build.yml)
![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)

Курсовая работа по дисциплине «Разработка приложений»

## Запуск

1. Клонировать репозиторий:
    - `git clone https://github.com/Albuka1/PressSubscription.git`
    - `cd PressSubscription`

2. Установить **.NET 8.0** SDK:
   `https://dotnet.microsoft.com/download/dotnet/8.0`

    - Проверить установку:
   `dotnet --version`

3. Восстановить пакеты и собрать проект:
   `dotnet restore`
   `dotnet build`

4. Запустить приложение:
   `dotnet run --project PressSubscription`

## Импорт данных из Excel

1. Взять файл `ImportTemplate.xlsx` (лежит в папке с проектом)
2. Заполнить данными (примеры уже есть)
3. В приложении: кнопка **"Импорт из Excel"** → выбрать файл

## Структура Excel-файла

Файл должен содержать 3 листа:

### Лист `Publications`
| Id | Title | Publisher | PricePerMonth | ImagePath |
|----|-------|-----------|---------------|-----------|
| 1 | Новости | Изд-во А | 250 | Images/news.png |

### Лист `Subscribers`
| Id | FullName | Address | Phone | Email |
|----|----------|---------|-------|-------|
| 1 | Иванов И. | ул. Ленина 10 | 1234567 | ivan@mail.ru |

### Лист `Subscriptions`
| Id | SubscriberId | PublicationId | StartDate | Months | BaseCost | DeliveryCost | Vat | TotalCost |
|----|--------------|---------------|-----------|--------|----------|--------------|-----|-----------|
| 1 | 1 | 1 | 01.04.2026 | 6 | 1500 | 15 | 270 | 1785 |

> **Формулы:**  
> DeliveryCost = BaseCost × 0.01 (1% доставка)  
> Vat = BaseCost × 0.18 (НДС 18%)  
> TotalCost = BaseCost + DeliveryCost + Vat

## Примечание

- Изображения изданий класть в папку `Images` рядом с `PressSubscription.exe`
- Поддерживаются `.png`, `.jpg`
- Если картинки нет - покажется `placeholder.png`

