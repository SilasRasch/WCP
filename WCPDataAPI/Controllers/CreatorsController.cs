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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreatorUser>>> Get([FromQuery] int? orderId = null, [FromQuery] string? searchTerm = null)
        {
            List<Creator> creators = await _creatorService.GetAllObjects();

            if (orderId is not null)
            {
                Order? order = await _orderService.GetObject(orderId.Value);

                if (order is not null && order.Creators is not null)
                {
                    creators = creators.Where(order.Creators.Contains).ToList();
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

            List<CreatorUser> combined = new List<CreatorUser>();

            foreach (Creator creator in creators)
            {
                User? user = await _userService.GetObject(creator.UserId);
                if (user is not null)
                {
                    combined.Add(DtoConverter.UserCreatorToCreatorUser(user, creator));
                }
            }

            return combined is not null ? Ok(combined) : NotFound("No creators found");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CreatorUser>> Get(int id)
        {
            Creator? creator = await _creatorService.GetObject(id);

            if (creator is null)
                return NotFound();

            User? user = await _userService.GetObject(creator.UserId);
            if (user is not null)
            {
                return Ok(DtoConverter.UserCreatorToCreatorUser(user, creator));
            }

            return NotFound();
        }

        [HttpGet("GetByEmail/{email}")]
        public async Task<ActionResult<CreatorUser>> GetByEmail(string email)
        {
            Creator? creator = (await _creatorService.GetAllCreatorsWithUser()).SingleOrDefault(x => x.User.Email == email);

            if (creator is null)
                return NotFound();

            User? user = await _userService.GetObject(creator.UserId);
            if (user is not null)
            {
                return Ok(DtoConverter.UserCreatorToCreatorUser(user, creator));
            }

            return NotFound();
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(CreatorDto creator)
        {
            var user = await _userService.GetObject(creator.UserId);

            if (!creator.Validate() || user is null)
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            await _creatorService.AddObject(creator);

            return CreatedAtAction("Post", new { id = creator.UserId }, creator);
        }

        [HttpPost("AddRange"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostRange([FromBody] CreatorDtoList request)
        {
            foreach (var creator in request.Creators)
            {
                var user = await _userService.GetObject(creator.UserId);

                if (!creator.Validate() || user is null)
                    return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

                await _creatorService.AddObject(creator);
            }

            return Ok();
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

            if (creator is null)
                return NotFound("Creator not found");

            if (creator.UserId != _userContextService.GetId() && !_userContextService.GetRoles().Contains("Admin"))
                return BadRequest("Du har ikke tilladelse til at ændre denne creator");

            Creator? deleted = await _creatorService.DeleteObject(id);
            return deleted is not null ? NoContent() : NotFound("Creator not found");
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
