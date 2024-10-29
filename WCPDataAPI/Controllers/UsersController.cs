using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WCPShared.Models.Entities.AuthModels;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Views;
using WCPShared.Services;
using WCPShared.Services.EntityFramework;

namespace WCPDataAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly UserContextService _userContextService;

        public UsersController(UserService userService, UserContextService userContextService)
        {
            _userService = userService;
            _userContextService = userContextService;
        }

        // GET: api/<UsersController>
        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserView>>> Get([FromQuery] string? role)
        {
            if (role is not null) return Ok(await _userService.GetObjectsViewBy(x => x.Role.ToLower() == role.ToLower()));
            return Ok(await _userService.GetAllObjectsView());
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserView>> Get(int id)
        {
            UserView? user = await _userService.GetObjectViewBy(x => x.Id == id);
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

            await _userService.UpdateObject(user.Id, request);
            return Ok();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_userContextService.GetId() != id && !_userContextService.GetRoles().Contains("Admin"))
                return Unauthorized();

            User? user = await _userService.GetObject(id);

            if (user is null)
                return NotFound();

            user.IsActive = !user.IsActive;
            user = await _userService.UpdateObject(id, user);
            return user is not null ? NoContent() : NotFound();
        }
    }
}
