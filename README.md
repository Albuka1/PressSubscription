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

## Первый запуск и учётные записи

При **первом запуске** приложение автоматически создаст базу данных и предложит создать учётную запись администратора.

### Создание администратора при первом запуске

1. Запустите `PressSubscription.exe`
2. Появится окно с предложением создать администратора
3. Нажмите **"Да"**, чтобы создать учётную запись администратора
4. После создания вы сможете войти с логином `admin` и паролем `admin`

### Таблица пользователей (users)

База данных содержит таблицу `Users` со следующей структурой:

| Поле | Тип | Описание |
|------|-----|----------|
| Id | INTEGER (PK) | Уникальный идентификатор пользователя |
| Username | TEXT | Логин пользователя (уникальный) |
| PasswordHash | TEXT | Хеш пароля (SHA256) |
| Role | TEXT | Роль: `Admin` или `User` |
| FullName | TEXT | Полное имя пользователя |
| Email | TEXT | Адрес электронной почты (опционально) |
| CreatedAt | DATETIME | Дата регистрации |
| LastLoginAt | DATETIME | Дата последнего входа |
| IsActive | BOOLEAN | Активна ли учётная запись (`1` — активна, `0` — заблокирована) |

> **Важно:** Пароли в базе данных хранятся в захешированном виде (SHA256).

### Учётные записи по умолчанию

| Логин | Пароль | Роль | Описание |
|-------|--------|------|----------|
| `admin` | `admin` | **Admin** | Полный доступ ко всем функциям системы |
| `user` | `user` | **User** | Только просмотр данных |

### Управление пользователями

- **Администратор** имеет доступ к панели администратора (вкладка "⚙️ Админ панель"), где может:
  - Назначать или снимать права администратора
  - Блокировать или разблокировать пользователей
- **Обычный пользователь** (`User`) может только просматривать данные.

## Примечание

- Изображения изданий класть в папку `Images` рядом с `PressSubscription.exe`
- Поддерживаются `.png`, `.jpg`
- Если картинки нет - покажется `placeholder.png`

