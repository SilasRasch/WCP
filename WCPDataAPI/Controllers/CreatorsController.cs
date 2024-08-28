using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WCPShared.Models.OrderModels;
using WCPShared.Models.UserModels.CreatorModels;
using WCPShared.Services;
using WCPShared.Interfaces.Mongo;
using WCPShared.Models.UserModels;
using WCPShared.Interfaces;

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

        public CreatorsController(UserContextService userContextService, ICreatorService creatorService, IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userContextService = userContextService;
            _creatorService = creatorService;
            _userService = userService;
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
                    var creatorList = await _creatorService.GetAllObjects(x => order.Creators.Contains(x.Id));

                    if (creatorList is not null)
                        creators = creatorList;
                }
            }

            if (searchTerm is not null)
            {
                // Use reflection to search through props
                searchTerm = searchTerm.Trim().ToLower();
                creators = creators.Where(x => x.Email.ToLower().Contains(searchTerm) ||
                    x.Name.ToLower().Contains(searchTerm)
                    ).ToList();
            }

            return creators is not null ? Ok(creators) : NotFound("No creators found");
        }

        [HttpGet("/api/creators-with-user")]
        public async Task<ActionResult<Dictionary<User, Creator>>> GetCreatorUsers()
        {
            IEnumerable<Creator> creators = await _creatorService.GetAllObjects();
            Dictionary<User, Creator> keyValuePairs = new Dictionary<User, Creator>();

            foreach (Creator creator in creators) 
            {
                User? user = await _userService.GetObject(creator.Id);
                if (user is not null)
                    keyValuePairs.Add(user, creator);
            }

            return keyValuePairs;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Creator>> Get(int id)
        {
            Creator? creator = await _creatorService.GetObject(id);
            return creator is not null ? Ok(creator) : NotFound();
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(Creator creator)
        {
            if (!creator.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            await _creatorService.AddObject(creator);
            return CreatedAtAction("Post", new { id = creator.Id }, creator);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Creator>> Put(Creator creator, int id)
        {
            if (!creator.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            if (creator.Id != _userContextService.GetId() && !_userContextService.GetRoles().Contains("Admin"))
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
