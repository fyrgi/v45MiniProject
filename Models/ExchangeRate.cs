using System;
using System.Collections.Generic;
using System.Text;

namespace v45MiniProject.Models
{
    public class ExchangeRate
    {
        public decimal Rate { get; set; }
        public required string Source { get; set; }
    }
}
