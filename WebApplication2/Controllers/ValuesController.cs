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
        public IEnumerable<User> GetAll()
        {
           
            return userStorage.GetAll();
          
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<User> GetbyId(int id)
        {
            //User user;
            User tmp = userStorage.Find(id);
            if (tmp == null)
            {
               return NotFound();
            }
            else
            {
                return tmp;
            }
          
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] User user)
        {
            User tmp = userStorage.Find(user.Id);
            if (tmp != null)
            {
                return BadRequest();
            }
            else
            {
                userStorage.AddUser(user);
                return Ok();
            }
           
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            

            User tmp = userStorage.Find(id);
            if (tmp != null)
            {
                User user = new User(id, value);
                userStorage.Update(user);
                return Ok();
            }
            else
            {
                return NotFound();
            }
          
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {

            if (userStorage.RemoveUser(id))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }

           /* User tmp = userStorage.Find(id);
            if (tmp == null)
            {
                return NotFound();
            }
            else
            {
                userStorage.RemoveUser(id);
                return Ok();
            }*/

           
        }
    }
}
