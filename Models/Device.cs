using System;
using System.Collections.Generic;
using System.Text;

namespace v45MiniProject.Models
{
    internal class Device : IDevice
    {
        // Important empty constructor for deserialization of JSON data.
        public Device()
        {

        }
        public Device(
            int id
            , int officeId
            , DeviceType deviceType
            , string brand
            , string model
            , double priceMainOffice
            , string currencyMainOffice
            , double priceLocalOffice
            , string currencyLocalOffice
            , string sourceOfCurrency
            , DateTime purchaseDate
            )
        {
            Id = id;
            DeviceType = deviceType;
            OfficeId = officeId;
            Brand = brand;
            Model = model;
            PriceMainOffice = priceMainOffice;
            CurrencyMainOffice = currencyMainOffice;
            PriceLocalOffice = priceLocalOffice;
            CurrencyLocalOffice = currencyLocalOffice;
            SourceOfCurrency = sourceOfCurrency;
            PurchaseDate = purchaseDate;
            EndOfLifeDate = purchaseDate.AddYears(3);
        }
        public int Id { get; set; }
        public DeviceType DeviceType { get; set; }
        public int OfficeId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public double PriceMainOffice { get; set; }
        public string CurrencyMainOffice { get; set; }
        public double PriceLocalOffice { get; set; }
        public string CurrencyLocalOffice { get; set; }
        public string SourceOfCurrency { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime EndOfLifeDate { get; set; }
        public DeviceType Type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
