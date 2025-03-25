using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

// hello world!

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{

    private static List<User> _users = new List<User>
    {
        new User { Id = 0, Username = "ken", Email = "user1@gmail.com", Password = "password1" },
        new User { Id = 1, Username = "bob", Email = "user2@gmail.com", Password = "password2" },
        new User { Id = 2, Username = "mike", Email =  "user3@gmail.com", Password = "password3" }
    };

    private static readonly object _lock = new object();

    [HttpGet]
    public ActionResult<List<User>> GetUsers()
    {
        return Ok(_users);
    }

    [HttpGet("{id}")]
    public ActionResult<User> GetUserById(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public ActionResult<User> CreateUser([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        lock (_lock)
        {
            user.Id = _users.Count + 1;
            _users.Add(user);
        }
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public ActionResult<User> UpdateUser(int id, [FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        lock (_lock)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return NotFound();
            }
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
        }
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        lock (_lock)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            _users.Remove(user);
        }
        return NoContent();
    }
}