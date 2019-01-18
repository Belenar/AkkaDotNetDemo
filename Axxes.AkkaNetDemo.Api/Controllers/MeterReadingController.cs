using System;
using Axxes.AkkaNetDemo.Api.Models;
using Axxes.AkkaNetDemo.System;
using Axxes.AkkaNetDemo.System.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Axxes.AkkaNetDemo.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MeterReadingController : ControllerBase
    {
        public void Post([FromBody] MeterReading reading)
        {
            var message = new MeterReadingReceived()
            {
                DeviceId = reading.DeviceId,
                Timestamp = reading.Timestamp,
                MeterValue = reading.MeterValue,
                Unit = reading.Unit
            };

            var address = $"/user/devices/device-{reading.DeviceId}";

            var actorSelection = SystemInstance.Current.ActorSystem
                .ActorSelection(address);

            actorSelection.Tell(message);

            Console.WriteLine($"Meter reading for device {reading.DeviceId}");
        }
    }
}