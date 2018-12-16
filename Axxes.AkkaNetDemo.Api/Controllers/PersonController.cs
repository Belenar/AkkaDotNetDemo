using System;
using System.Collections.Generic;
using System.Linq;
using Axxes.AkkaNetDemo.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Axxes.AkkaNetDemo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private static List<Person> _persons = new List<Person>
        {
            new Person { Id = 1, FirstName = "Hannes", LastName = "Lowette", Birthday = new DateTime(1982, 2, 11)},
            new Person { Id = 2, FirstName = "John", LastName = "Doe", Birthday = new DateTime(1979, 1, 1)}
        };

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Person>> Get()
        {
            return _persons;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Person> Get(int id)
        {
            var value = _persons.FirstOrDefault(p => p.Id == id);

            if (value != null)
                return value;

            return NotFound(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Person person)
        {
            if (person == null)
                return;

            person.Id = _persons.Max(p => p.Id) + 1;

            _persons.Add(person);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Person person)
        {
            var value = _persons.FirstOrDefault(p => p.Id == id);

            if (value == null)
                return;

            value.FirstName = person.FirstName;
            value.LastName = person.LastName;
            value.Birthday = person.Birthday;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var value = _persons.FirstOrDefault(p => p.Id == id);

            if (value == null)
                return;

            _persons.Remove(value);
        }
    }
}
