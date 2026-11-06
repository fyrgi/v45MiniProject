using System;
using System.Collections.Generic;
using System.Text;

namespace v45MiniProject.Models
{
    internal interface IDevice
    {
        int Id { get; set; }
        DeviceType Type { get; set; }
        int OfficeId { get; set; }
        string Brand { get; set; }
        string Model { get; set; }
        double PriceMainOffice { get; set; }
        string CurrencyMainOffice { get; set; }
        double PriceLocalOffice { get; set; }
        string CurrencyLocalOffice { get; set; }
        string SourceOfCurrency { get; set; }
        DateTime PurchaseDate { get; set; }
        DateTime EndOfLifeDate { get; set; }
    }
}
