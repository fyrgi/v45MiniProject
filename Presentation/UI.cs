
using System;
using System.Collections.Generic;
using v45MiniProject.Models;
using v45MiniProject.Services;

namespace v45MiniProject.Presentation
{
    internal class UI
    {
        public static void ShowMessage(string message, string messageType)
        {
            switch (messageType)
            {
                case "error":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case "success":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case "info":
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;

                case "warning":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case "3orless":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;

                case "6orless":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;

                case "statistics":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;

                default:
                    Console.ResetColor();
                    break;
            }

            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine();
        }

        // This display is not currently in use.
        public static void ShowDevicesByOffice2(DeviceManager deviceManager)
        {
            DateTime today = DateTime.Today;

            ShowMessage(
                "ASSET TRACKING - DEVICES BY OFFICE",
                "info");

            foreach (Office office in deviceManager.GetOffices())
            {
                ShowMessage(
                    $"{office.Country.ToUpper()} ({office.Currency})",
                    "statistics");

                List<Device> devices = deviceManager.GetDevicesByOffice(office.Id);

                if (devices.Count == 0)
                {
                    ShowMessage("No devices found.", "info");
                    continue;
                }

                foreach (Device device in devices)
                {
                    DeviceLifeStatus status = deviceManager.GetDeviceLifeStatus(device, today);

                    string messageType = status switch
                    {
                        DeviceLifeStatus.Red => "3orless",
                        DeviceLifeStatus.Yellow => "6orless",
                        _ => "default"
                    };

                    string deviceInfo =
                        $"ID: {device.Id} | " +
                        $"{device.Brand} {device.Model}\n" +
                        $"Type: {device.DeviceType}\n" +
                        $"Purchase price: {device.PriceMainOffice:N2} " +
                        $"{device.CurrencyMainOffice}\n" +
                        $"Local price: {device.PriceLocalOffice:N2} " +
                        $"{device.CurrencyLocalOffice}\n" +
                        $"Exchange rate source: {device.SourceOfCurrency}\n" +
                        $"Purchase date: {device.PurchaseDate:yyyy-MM-dd}\n" +
                        $"End of life: {device.EndOfLifeDate:yyyy-MM-dd}";

                    if (status == DeviceLifeStatus.Red &&
                        device.EndOfLifeDate < today)
                    {
                        deviceInfo += "\nSTATUS: EXPIRED";
                    }

                    ShowMessage(deviceInfo, messageType);
                }
            }
        }

        // The display in use is a more compact table format for showing devices by office.
        public static void ShowDevicesByOffice(DeviceManager deviceManager)
        {
            DateTime today = DateTime.Today;

            foreach (Office office in deviceManager.GetOffices())
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"OFFICE: {office.Country} ({office.Currency})");
                Console.ResetColor();

                // Table header
                Console.WriteLine(new string('-', 135));

                Console.WriteLine(
                    $"{"ID",-5} {"Type",-12} {"Brand",-12} {"Model",-22} " +
                    $"{"Purchased",-16} {"Use before",-16}" +
                    $"{"Price USD",-17} {"Local price("+office.Currency+")",-18}" +
                    $"{"Status", - 9}");

                Console.WriteLine(new string('-', 135));

                List<Device> devices = deviceManager.GetDevicesByOffice(office.Id);

                if (devices.Count == 0)
                {
                    Console.WriteLine("No devices registered for this office.");
                }

                foreach (Device device in devices)
                {
                    DeviceLifeStatus status = deviceManager.GetDeviceLifeStatus(device, today);

                    Console.ForegroundColor = status switch
                    {
                        DeviceLifeStatus.Red => ConsoleColor.Red,
                        DeviceLifeStatus.Yellow => ConsoleColor.Yellow,
                        _ => ConsoleColor.White
                    };

                    string mainPrice = $"{device.PriceMainOffice:N2}";

                    string localPrice = $"{device.PriceLocalOffice:N2}";

                    string model = Fit(device.Model, 22);
                    string brand = Fit(device.Brand, 12);

                    string statusText = device.EndOfLifeDate < today ? " EXPIRED" : "";

                    Console.WriteLine(
                        $"{device.Id,-5} " +
                        $"{device.DeviceType,-12} " +
                        $"{brand,-12} " +
                        $"{model,-22} " +
                        $"{device.PurchaseDate:yyyy-MM-dd}\t" + 
                        $"{device.EndOfLifeDate:yyyy-MM-dd}\t" +
                        $"{mainPrice,-17} " +
                        $"{localPrice,-17}" +
                        statusText);

                    Console.ResetColor();
                }

                Console.WriteLine(new string('-', 135));
            }
        }

        private static string Fit(string value, int width)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            if (value.Length > width)
                return value.Substring(0, width - 3) + "...";

            return value;
        }
    }
}
