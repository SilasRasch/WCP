using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WCPShared.Models.OrderModels;
using WCPShared.Models.UserModels.CreatorModels;
using WCPShared.Services.Databases;
using WCPShared.Services;
using WCPShared.Services.StaticHelpers;

namespace WCPDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatorsController : ControllerBase
    {
        private readonly IMongoCollection<Creator> _creators;
        private readonly IMongoCollection<Order> _orders;
        private readonly UserContextService _userContextService;

        public CreatorsController(MongoDbService mongoDbService, UserContextService userContextService)
        {
            _creators = mongoDbService.Database?.GetCollection<Creator>(Secrets.MongoCreatorCollectionName)!;
            _orders = mongoDbService.Database?.GetCollection<Order>(Secrets.MongoCollectionName)!;
            _userContextService = userContextService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Creator>>> Get([FromQuery] int? orderId = null, [FromQuery] string? searchTerm = null)
        {
            List<Creator> creators = null!;

            if (orderId is null)
            {
                creators = await _creators.Find(FilterDefinition<Creator>.Empty).ToListAsync();
            }
            else
            {
                Order? order = await _orders.Find(x => x.Id == orderId.Value).FirstOrDefaultAsync();

                if (order is not null && order.Creators is not null)
                {
                    var creatorList = _creators.Find(x => order.Creators!.Contains(x.Id));

                    if (creatorList is not null)
                        creators = await creatorList.ToListAsync();
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

            return creators is not null ? Ok(creators) : NotFound("");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Creator>> Get(int id)
        {
            Creator? creator = await _creators.Find(x => x.Id == id).FirstOrDefaultAsync();
            return creator is not null ? Ok(creator) : NotFound();
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(Creator creator)
        {
            if (!creator.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            await _creators.InsertOneAsync(creator);
            return CreatedAtAction("Post", new { id = creator.Id }, creator);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Creator>> Put(Creator creator, int id)
        {
            if (creator.Id != _userContextService.GetId() && !_userContextService.GetRoles().Contains("Admin"))
                return BadRequest("Du har ikke tilladelse til at ændre denne creator");

            if (!creator.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            if (id != creator.Id)
                return BadRequest("Ids must match in URI and body");

            ReplaceOneResult result = await _creators.ReplaceOneAsync(x => x.Id == creator.Id, creator);

            if (result.IsAcknowledged)
                return result.ModifiedCount == 1 ? Ok(result) : NotFound();

            return NotFound();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Creator? creator = await _creators.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (creator is not null)
            {
                if (creator.Id != _userContextService.GetId() && !_userContextService.GetRoles().Contains("Admin"))
                    return BadRequest("Du har ikke tilladelse til at ændre denne creator");
            }
            else return NotFound();

            DeleteResult result = await _creators.DeleteOneAsync(x => x.Id == id);

            if (result.IsAcknowledged)
                return result.DeletedCount == 1 ? Ok(result) : NotFound();

            return NotFound();
        }
    }
}
