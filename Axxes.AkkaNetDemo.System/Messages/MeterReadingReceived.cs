using System;

namespace Axxes.AkkaNetDemo.System.Messages
{
    public class MeterReadingReceived
    {
        public Guid DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal MeterValue { get; set; }
        public string Unit { get; set; }
    }
}
