using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WCPShared.Interfaces;
using WCPShared.Models.OrderModels;
using WCPShared.Services.Databases;
using WCPShared.Services;
using WCPShared.Services.StaticHelpers;

namespace WCPDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMongoCollection<Order> _orders;
        private readonly IEmailService _emailService;
        private readonly UserContextService _userContextService;

        public OrdersController(MongoDbService mongoDbService, IEmailService emailService, UserContextService userContextService)
        {
            _orders = mongoDbService.Database?.GetCollection<Order>(Secrets.MongoCollectionName)!;
            _emailService = emailService;
            _userContextService = userContextService;
        }


        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get([FromQuery] int? userId = null, [FromQuery] int? creatorId = null, [FromQuery] int? orgId = null)
        {
            if (orgId is not null) // && creatorId is null && userId is null)
                return Ok(await _orders.Find(x => x.OrganizationId == (int)orgId).ToListAsync());
            else if (_userContextService.GetRoles().Contains("Bruger")) // Catch (get by JWT role)
                return Ok(await _orders.Find(x => x.OrganizationId == _userContextService.GetOrganizationId()).ToListAsync());

            if (creatorId is not null) // && userId is null)
                return Ok(await _orders.Find(x => x.Creators!.Contains((int)creatorId)).ToListAsync());
            else if (_userContextService.GetRoles().Contains("Creator") || _userContextService.GetRoles().Contains("Editor")) // Catch (get by JWT role)
                return Ok(await _orders.Find(x => x.Creators!.Contains(_userContextService.GetId())).ToListAsync());

            if (_userContextService.GetRoles().Contains("Admin") && userId is null && creatorId is null && orgId is null)
                return Ok(await _orders.Find(FilterDefinition<Order>.Empty).ToListAsync());

            if (userId is not null) // && creatorId is null)
                return Ok(await _orders.Find(x => x.UserId == userId).ToListAsync());

            return Ok(new List<Order>());
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(int id)
        {
            Order? order = await _orders.Find(x => x.Id == id).FirstOrDefaultAsync();

            return order is not null ? Ok(order) : NotFound();
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<ActionResult<Order>> Post(Order order)
        {
            if (!order.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            // make sure numbered ids are used
            Order lastOrder;
            if (await _orders.Find(FilterDefinition<Order>.Empty).CountDocumentsAsync() != 0)
                lastOrder = await _orders.Find(FilterDefinition<Order>.Empty).SortByDescending(o => o.Id).Limit(1).FirstAsync();
            else
                lastOrder = null!;

            order.Id = lastOrder != null ? lastOrder.Id + 1 : 1000;

            await _orders.InsertOneAsync(order);

            return CreatedAtAction("Post", new { id = order.Id }, order);
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Order>> Put(Order order, int id)
        {
            if (order.OrganizationId != _userContextService.GetOrganizationId() && !_userContextService.GetRoles().Contains("Admin"))
                return Unauthorized("Du har ikke tilladelse til at ændre denne ordre");

            if (!order.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            if (id != order.Id)
                return BadRequest("Ids must match in URI and body");

            // Determine whether or not to send notification email
            Order? oldOrder = await _orders.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (oldOrder is not null)
            {
                string projectCategory = string.Empty;
                if (oldOrder.Status.Category == 1 && order.Status.Category == 2)
                    projectCategory = "Planlægning";
                else if (oldOrder.Status.Category == 3 && order.Status.Category == 4)
                    projectCategory = "Feedback";

                await _emailService.SendNotificationEmail(order.Contact.Name, order.Contact.Email, order.ProjectName, projectCategory);

                order.UserId = oldOrder.UserId; // Never change user ID
            }

            ReplaceOneResult result = await _orders.ReplaceOneAsync(x => x.Id == order.Id, order);

            if (result.IsAcknowledged)
                return result.ModifiedCount == 1 ? Ok(result) : NotFound();

            return NotFound();
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Order? order = await _orders.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (order is not null)
                return NotFound();

            if (!_userContextService.GetRoles().Contains("Admin")) // Users cannot delete orders
                return Unauthorized("Du har ikke tilladelse til at ændre denne ordre");

            DeleteResult result = await _orders.DeleteOneAsync(x => x.Id == id);

            if (result.IsAcknowledged)
                return result.DeletedCount == 1 ? Ok(result) : NotFound();

            return NotFound();
        }
    }
}
