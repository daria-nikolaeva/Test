using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IUserStorage userStorage;

        public ValuesController(IUserStorage storage)
        {
            userStorage = storage;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return userStorage.Instance.Values.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            User user;
            userStorage.Instance.TryGetValue(id, out user);

            return user;
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] User user)
        {
            if (userStorage.Instance.ContainsKey(user.Id))
            {
                return BadRequest();
            }
            else
            {
                userStorage.Instance.Add(user.Id, user);
                return Ok();
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            if (userStorage.Instance.ContainsKey(id))
            {
                userStorage.Instance[id] = new User(id, value);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (userStorage.Instance.ContainsKey(id))
            {
                userStorage.Instance.Remove(id);
                return this.Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
