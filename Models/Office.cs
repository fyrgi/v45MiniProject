using System;
using System.Collections.Generic;
using System.Text;

namespace v45MiniProject.Models
{
    internal class Office
    {
        public Office() { }

        public Office(int id,string country, string currency)
        {
            Id = id;
            Country = country;
            Currency = currency;
        }
        public int Id { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
    }
}
