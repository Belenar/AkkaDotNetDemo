using System;
using Akka.Actor;
using Axxes.AkkaNetDemo.System;
using Axxes.AkkaNetDemo.System.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Axxes.AkkaNetDemo.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<Guid> Get(Guid id)
        {
            Console.WriteLine($"Hello for device {id}");

            var message = new RegisterDevice {DeviceId = id};

            SystemInstance.Current.DevicesActor.Tell(message, Nobody.Instance);

            return Ok(id);
        }
    }
}