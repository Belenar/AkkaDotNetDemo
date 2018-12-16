using System;
using Axxes.AkkaNetDemo.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Axxes.AkkaNetDemo.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MeterReadingController : ControllerBase
    {
        public void Post([FromBody] MeterReading reading)
        {
            Console.WriteLine($"Meter reading for device {reading.DeviceId}");
        }
    }
}