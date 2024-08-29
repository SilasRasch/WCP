using Microsoft.AspNetCore.Mvc;
using WCPShared.Models.OrderModels;
using WCPShared.Services;
using WCPShared.Interfaces.Mongo;
using Microsoft.AspNetCore.Authorization;

namespace WCPDataAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly UserContextService _userContextService;

        public OrdersController(UserContextService userContextService, IOrderService orderService)
        {
            _userContextService = userContextService;
            _orderService = orderService;
        }


        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderMongo>>> Get([FromQuery] int? userId = null, [FromQuery] int? creatorId = null, [FromQuery] int? orgId = null)
        {
            if (orgId is not null) // && creatorId is null && userId is null)
                return Ok(await _orderService.GetAllObjects(x => x.OrganizationId == (int)orgId));
            else if (_userContextService.GetRoles().Contains("Bruger")) // Catch (get by JWT role)
                return Ok(await _orderService.GetAllObjects(x => x.OrganizationId == _userContextService.GetOrganizationId()));

            if (creatorId is not null) // && userId is null)
                return Ok(await _orderService.GetAllObjects(x => x.Creators!.Contains((int)creatorId)));
            else if (_userContextService.GetRoles().Contains("Creator") || _userContextService.GetRoles().Contains("Editor")) // Catch (get by JWT role)
                return Ok(await _orderService.GetAllObjects(x => x.Creators!.Contains(_userContextService.GetId())));

            if (_userContextService.GetRoles().Contains("Admin") && userId is null && creatorId is null && orgId is null)
                return Ok(await _orderService.GetAllObjects());

            if (userId is not null) // && creatorId is null)
                return Ok(await _orderService.GetAllObjects(x => x.UserId == userId));

            return Ok(new List<OrderMongo>());
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderMongo>> Get(int id)
        {
            OrderMongo? order = await _orderService.GetObject(id);
            return order is not null ? Ok(order) : NotFound();
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<ActionResult<OrderMongo>> Post(OrderMongo order)
        {
            if (!order.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            await _orderService.AddObject(order);

            return Created();
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderMongo>> Put(OrderMongo order, int id)
        {
            if (!order.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            if (order.OrganizationId != _userContextService.GetOrganizationId() && !_userContextService.GetRoles().Contains("Admin"))
                return Unauthorized("Du har ikke tilladelse til at ændre denne ordre");

            if (id != order.Id)
                return BadRequest("Ids must match in URI and body");

            OrderMongo? modifiedOrder = await _orderService.UpdateObject(id, order);
            return modifiedOrder is not null ? NoContent() : NotFound("Order not found");
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            OrderMongo? order = await _orderService.GetObject(id);

            if (order is not null)
                return NotFound();

            if (!_userContextService.GetRoles().Contains("Admin")) // Users cannot delete orders
                return Unauthorized("Du har ikke tilladelse til at ændre denne ordre");

            OrderMongo? deleted = await _orderService.DeleteObject(id);
            return deleted is not null ? NoContent() : NotFound("Order not found");
        }
    }
}
