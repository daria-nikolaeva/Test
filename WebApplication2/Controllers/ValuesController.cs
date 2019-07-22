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
           
            User tmp = userStorage.FindUser(id);
           
            if (!userStorage.ContainsUser(id))
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
            
            if (userStorage.ContainsUser(user.Id))
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

          
           
            if (userStorage.ContainsUser(id)&&userStorage.FindUser(id).Name!=value)
            {
                User user = new User(id, value);
                userStorage.UpdateUser(user);
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

            if (userStorage.ContainsUser(id))
            {
                userStorage.RemoveUser(id);
                return Ok();
            }
            else
            {
                return NotFound();
            }

           

           
        }
    }
}
