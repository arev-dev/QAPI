using Microsoft.AspNetCore.Mvc;
using QAPI.Models;
using QAPI.Services.Interfaces;

namespace QAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user.Id == 0)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<int> CreateUser([FromBody] User user)
        {
            var newUserId = _userService.CreateUser(user);
            return CreatedAtAction(nameof(GetUserById), new { id = newUserId }, user);
        }
    }
}