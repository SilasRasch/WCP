using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models.AuthModels;
using WCPShared.Models.UserModels;
using WCPShared.Services;

namespace WCPDataAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserContextService _userContextService;

        public UsersController(IUserService userService, UserContextService userContextService)
        {
            _userService = userService;
            _userContextService = userContextService;
        }

        // GET: api/<UsersController>
        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> Get([FromQuery] string? role)
        {
            IEnumerable<User> users = await _userService.GetAllObjects();
            if (role is not null) users = users.Where(x => x.Role.ToLower() == role.ToLower());

            IEnumerable<User> ncUsers = users.Select((user) => new User()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                IsActive = user.IsActive,
                Phone = user.Phone,
                Role = user.Role,
                Organization = user.Organization,
            });

            return Ok(ncUsers);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> Get(int id)
        {
            User? user = await _userService.GetObject(id);
            return user is not null ? Ok(user) : NotFound("Der blev ikke fundet en bruger med det id...");
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RegisterDto request)
        {
            if (_userContextService.GetId() != id && !_userContextService.GetRoles().Contains("Admin"))
                return Unauthorized();

            User? user = await _userService.GetObject(id);

            if (user is null) return BadRequest();

            user.Email = request.Email;
            user.Name = request.Name;
            user.Phone = request.Phone;

            await _userService.UpdateObject(user.Id, user);
            return Ok();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_userContextService.GetId() != id && !_userContextService.GetRoles().Contains("Admin"))
                return Unauthorized();

            User? user = await _userService.DeleteObject(id);

            return user is null ? NotFound() : NoContent();
        }
    }
}
