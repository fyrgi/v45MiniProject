# v45MiniProject

## Overview

`v45MiniProject` is a .NET 10 console application for tracking company devices across offices. It reads device and office data from JSON files, calculates device end-of-life dates, and presents the inventory grouped by office.

The project is intentionally small at this stage, but its folders separate models, data handling, business services, and console presentation so that it can grow without placing all functionality in `Program.cs`.

## Current functionality

The application currently:

- Loads devices from `Data/assets.json`.
- Loads offices from `Data/offices.json`.
- Creates the data files with an empty JSON array if they do not exist.
- Finds devices belonging to each office using `OfficeId`.
- Sorts devices by office and end-of-life date.
- Calculates the end-of-life date as three years after the purchase date.
- Classifies device life status as:
  - `Normal`: more than six months until end of life.
  - `Yellow`: six months or less until end of life.
  - `Red`: three months or less until end of life, including expired devices.
- Displays an inventory table in the console with device type, brand, model, dates, prices, currencies, and status colors.
- Provides currency-rate and currency-conversion classes as the foundation for local-price calculations.

At startup, the current program displays the inventory once and then waits for input before exiting. It does not yet provide an interactive menu.

<img width="1108" height="541" alt="image" src="https://github.com/user-attachments/assets/dd3d56dc-4163-46f8-ac3b-96931842077a" />

## Project structure

```text
v45MiniProject/
├── Data/
│   ├── assets.json
│   └── offices.json
├── Models/
│   ├── Device.cs
│   ├── DeviceLifeStatus.cs
│   ├── DeviceType.cs
│   ├── ExchangeRate.cs
│   ├── IDevice.cs
│   └── Office.cs
├── Presentation/
│   └── UI.cs
├── Services/
│   ├── CurrencyService.cs
│   ├── DeviceManager.cs
│   ├── ExchangeRateApiClient.cs
│   └── FileHandler.cs
├── Program.cs
└── v45MiniProject.csproj
```

### Models

- `Device` represents an asset and contains its office, type, manufacturer, model, purchase information, prices, currencies, and end-of-life date.
- `Office` represents an office with an ID, country, and local currency.
- `DeviceType` defines the supported device categories: laptop, desktop, tablet, smartphone, and other.
- `DeviceLifeStatus` defines the display status used for end-of-life warnings.
- `ExchangeRate` represents a rate and its source.
- `IDevice` defines the common device properties. It can be expanded if several device implementations are introduced later.

### Services

- `FileHandler` provides generic JSON file reading and writing.
- `DeviceManager` provides device and office queries, ordering, and end-of-life status calculations.
- `CurrencyService` currently uses configured exchange rates and converts prices to a local currency.
- `ExchangeRateApiClient` provides an HTTP client for retrieving historical exchange rates. It is prepared for API integration, but the current startup flow still uses the rates configured by `CurrencyService`.

### Presentation

`UI` contains console-specific behavior such as table formatting, colors, messages, and device display. Services do not need to know how their results are rendered, which makes it possible to replace or extend the console interface later.

## Class relationships

An office is not a type of device, so devices do not inherit from `Office`. The relationship is represented by `Device.OfficeId`, which links a device to an office.

If the application later introduces separate `Laptop`, `Tablet`, or `Smartphone` classes, they can inherit from a common abstract `Device` base class. Shared behavior such as the three-year end-of-life rule would remain in that base class, while device-specific properties would remain in the derived classes. For the current JSON-driven application, `DeviceType` is sufficient and avoids unnecessary subclasses.

## Scaling plan

The project can grow in stages:

### 1. Complete the console application

- Add an interactive menu and keep the application running until the user chooses Exit.
- Add commands to list all devices, list devices by office, and filter by life status or device type.
- Add device and office validation before saving data.
- Move file paths and other settings into configuration rather than keeping them in startup code.

### 2. Improve the application layer

- Introduce interfaces such as `IDeviceRepository`, `IOfficeRepository`, and `IExchangeRateProvider`.
- Separate file persistence from business operations so `DeviceManager` can work with JSON, a database, or test data.
- Add dependency injection and asynchronous file/API operations.
- Add unit tests for end-of-life calculations, sorting, filtering, validation, and currency conversion.

### 3. Add external exchange-rate API support

The existing `ExchangeRateApiClient` can become an implementation of an exchange-rate provider. `CurrencyService` can then:

1. Check whether the source and target currencies are the same.
2. Request the historical rate for the purchase date.
3. Use the nearest available earlier date when the requested date is unavailable.
4. Fall back to configured rates when the API is unavailable.
5. Store the rate source with the calculated local price.

The HTTP client should be registered with `HttpClientFactory`, with its base URL and timeout supplied through configuration. API failures should be handled by the service rather than by the presentation layer.

### 4. Move to persistent storage or an API

When JSON files are no longer sufficient, the data layer can be replaced without changing the console commands. Possible next steps are:

- SQLite or another relational database for device and office records.
- A REST API exposing devices, offices, statuses, and currency operations.
- A separate web or desktop client using the same application and domain services.

The current console application can remain a client of that API, or the API can be added as a separate project in the solution.

## Possible console menu

An interactive menu would be a natural next step. A simple first version could be:

```text
Device Inventory
---------------
1. Show all devices
2. Show devices by office
3. Show devices nearing end of life
4. Filter devices by type
5. Add a device
6. Edit a device
7. Remove a device
8. Refresh exchange rates
0. Exit
```

The menu should be implemented in the `Presentation` folder. It should read and validate user input, call methods on services, and pass the returned results to display methods. It should not contain file access, currency calculations, or end-of-life business rules.

A possible flow is:

```text
Program
  -> Menu
	  -> DeviceManager / CurrencyService
		  -> FileHandler / ExchangeRateApiClient
	  -> UI display methods
```

This keeps the menu replaceable. For example, the same service methods could later be called from a web API instead of from the console menu.

## Running the project

From the project directory:

```bash
dotnet run
```

The project targets `.NET 10` and requires the .NET 10 SDK.
