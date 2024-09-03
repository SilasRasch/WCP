using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WCPShared.Services;
using WCPShared.Models.UserModels;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using WCPShared.Models.DTOs;
using System.Linq;

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
        public async Task<ActionResult<IEnumerable<Creator>>> Get([FromQuery] int? orderId = null, [FromQuery] string? searchTerm = null)
        {
            IEnumerable<Creator> creators = null!;

            if (orderId is null)
            {
                creators = await _creatorService.GetAllObjects();
            }
            else
            {
                Order? order = await _orderService.GetObject(orderId.Value);

                if (order is not null && order.Creators is not null)
                {
                    var creatorList = await _creatorService.GetAllObjects();
                    creatorList = creatorList.Where(order.Creators.Contains).ToList();

                    if (creatorList is not null)
                        creators = creatorList;
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

        [HttpGet("/api/creators-with-user")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetCreatorUsers()
        {
            IEnumerable<Creator> creators = await _creatorService.GetAllObjects();
            List<dynamic> combined = new List<dynamic>();

            foreach (Creator creator in creators) 
            {
                User? user = await _userService.GetObject(creator.UserId);
                if (user is not null)
                {
                    UserNC userNC = user.ConvertToNCUser();
                    combined.Add(new { userNC, creator });
                }
            }

            return combined;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Creator>> Get(int id)
        {
            Creator? creator = await _creatorService.GetObject(id);
            return creator is not null ? Ok(creator) : NotFound();
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

        [HttpPut("{id}")]
        public async Task<ActionResult<Creator>> Put(CreatorDto creator, int id)
        {
            Creator? oldCreator = await _creatorService.GetObject(id);

            if (oldCreator is null)
                return NotFound("Creator not found");

            if (!creator.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            if (id != _userContextService.GetId() && !_userContextService.GetRoles().Contains("Admin"))
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

            if (creator.Id != _userContextService.GetId() && !_userContextService.GetRoles().Contains("Admin"))
                return BadRequest("Du har ikke tilladelse til at ændre denne creator");

            Creator? deleted = await _creatorService.DeleteObject(id);
            return deleted is not null ? NoContent() : NotFound("Creator not found");
        }
    }
}
