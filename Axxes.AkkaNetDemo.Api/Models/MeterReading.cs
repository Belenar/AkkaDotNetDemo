using System;

namespace Axxes.AkkaNetDemo.Api.Models
{
    public class MeterReading
    {
        public Guid DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal MeterValue { get; set; }
        public string Unit { get; set; }
    }
}