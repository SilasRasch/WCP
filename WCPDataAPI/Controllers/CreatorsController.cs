using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WCPShared.Services;
using WCPShared.Models.UserModels;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using WCPShared.Models.DTOs;
using WCPShared.Services.StaticHelpers;
using MongoDB.Driver;
using EllipticCurve.Utils;
using WCPShared.Models.DTOs.RangeDTOs;
using WCPShared.Models.Views;

namespace WCPDataAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CreatorsController : ControllerBase
    {
        private readonly UserContextService _userContextService;
        private readonly ICreatorService _creatorService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly ILanguageService _languageService;

        public CreatorsController(UserContextService userContextService, ICreatorService creatorService, IOrderService orderService, IUserService userService, ILanguageService languageService)
        {
            _orderService = orderService;
            _userContextService = userContextService;
            _creatorService = creatorService;
            _userService = userService;
            _languageService = languageService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CreatorView>>> Get([FromQuery] int? orderId = null, [FromQuery] string? searchTerm = null)
        {
            List<CreatorView> creators = await _creatorService.GetAllObjectsView();

            if (orderId is not null)
            {
                Order? order = await _orderService.GetObject(orderId.Value);

                if (order is not null && order.Creators is not null)
                {
                    creators = creators.Where(x => order.Creators.Any(c => c.Id == x.Id)).ToList();
                }
            }

            if (searchTerm is not null)
            {
                // Use reflection to search through props
                searchTerm = searchTerm.Trim().ToLower();
                creators = creators.Where(x => x.User.Email.ToLower().Contains(searchTerm) ||
                    x.User.Name.ToLower().Contains(searchTerm)
                    ).ToList();
            }

            return creators is not null ? Ok(creators) : NotFound("No creators found");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CreatorView>> Get(int id)
        {
            CreatorView? creator = await _creatorService.GetObjectViewBy(x => x.Id == id);

            if (creator is null)
                return NotFound();

            return Ok(creator);
        }

        [HttpGet("GetByEmail/{email}")]
        public async Task<ActionResult<CreatorView>> GetByEmail(string email)
        {
            CreatorView? creator = await _creatorService.GetObjectViewBy(x => x.User.Email == email);

            if (creator is null)
                return NotFound();

            return Ok(creator);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Creator>> Put(CreatorDto creator, int id)
        {
            Creator? oldCreator = await _creatorService.GetObject(id);

            if (oldCreator is null)
                return NotFound("Creator not found");

            if (!creator.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            if (oldCreator.UserId != _userContextService.GetId() && !_userContextService.GetRoles().Contains("Admin"))
                return BadRequest("You are not the owner of this creator");
                
            Creator? modifiedCreator = await _creatorService.UpdateObject(id, creator);
            return modifiedCreator is not null ? NoContent() : NotFound("Creator not found");
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Creator? creator = await _creatorService.GetObject(id);
            if (creator is null) return NotFound("Creator not found");

            User? user = await _userService.GetObject(creator.UserId);
            if (user is null) return NotFound("User not found");

            if (creator.UserId != _userContextService.GetId() && !_userContextService.GetRoles().Contains("Admin"))
                return BadRequest("Du har ikke tilladelse til at ændre denne creator");

            user.IsActive = false;
            user = await _userService.UpdateObject(user.Id, user);
            return user is not null && !user.IsActive ? NoContent() : NotFound("Creator not found");
        }

        [HttpPut("UpdateProfilePicture/{id}")]
        public async Task<IActionResult> UpdateProfilePicture(int id, [FromBody] string imgURL)
        {
            Creator? creator = (await _creatorService.GetAllObjects()).Where(x => x.UserId == id).SingleOrDefault();

            if (creator is null)
                return NotFound("Creator not found");

            if (creator.UserId != _userContextService.GetId() && !_userContextService.GetRoles().Contains("Admin"))
                return BadRequest("Du har ikke tilladelse til at ændre denne creator");

            creator.ImgURL = imgURL;

            if (!creator.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            Creator? modifiedCreator = await _creatorService.UpdateObject(creator.Id, creator);
            return modifiedCreator is not null ? NoContent() : NotFound("Creator not found");
        }
    }
}
