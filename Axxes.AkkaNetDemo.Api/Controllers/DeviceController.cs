using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
            return Ok(id);
        }
    }
}