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
            var message = new MeterReadingReceived
            {
                DeviceId = reading.DeviceId,
                Timestamp = reading.Timestamp,
                MeterValue = reading.MeterValue,
                Unit = reading.Unit
            };

            var actorRef = 
                SystemInstance.Current.ActorSystem
                    .ActorSelection($"/user/devices/device-{reading.DeviceId}");

            actorRef.Tell(message);
        }
    }
}