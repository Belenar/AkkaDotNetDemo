using System;

namespace Axxes.AkkaNetDemo.System.Messages
{
    public class QuarterCompleted
    {
        public Guid DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Consumption { get; set; }
        public string Unit { get; set; }
    }
}
