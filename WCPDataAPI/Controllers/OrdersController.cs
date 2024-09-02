using Microsoft.AspNetCore.Mvc;
using WCPShared.Services;
using Microsoft.AspNetCore.Authorization;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models;

namespace WCPDataAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly UserContextService _userContextService;
        private readonly IOrderService _orderService;

        public OrdersController(UserContextService userContextService, IOrderService orderService)
        {
            _userContextService = userContextService;
            _orderService = orderService;
        }


        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get([FromQuery] int? userId = null, [FromQuery] int? creatorId = null, [FromQuery] int? orgId = null)
        {
            var orders = await _orderService.GetAllObjects();

            if (orgId is not null) // && creatorId is null && userId is null)
                return Ok(orders.Where(x => x.Brand.OrganizationId == orgId.Value));
                
            else if (_userContextService.GetRoles().Contains("Bruger")) // Catch (get by JWT role)
                return Ok(orders.Where(x => x.Brand.OrganizationId == _userContextService.GetOrganizationId()));

            if (creatorId is not null)
                return Ok(orders.Where(x => x.Creators.Any(x => x.Id == creatorId.Value)));
            else if (_userContextService.GetRoles().Contains("Creator") || _userContextService.GetRoles().Contains("Editor")) // Catch (get by JWT role)
                return Ok(orders.Where(x => x.Creators!.Any(x => x.Id == _userContextService.GetId())));

            if (_userContextService.GetRoles().Contains("Admin") && userId is null && creatorId is null && orgId is null)
                return Ok(orders);

            return Ok(new List<Order>());
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(int id)
        {
            Order? order = await _orderService.GetObject(id);
            return order is not null ? Ok(order) : NotFound();
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<ActionResult<Order>> Post(Order order)
        {
            if (!order.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            await _orderService.AddObject(order);

            return Created();
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Order>> Put(Order order, int id)
        {
            if (!order.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            if (order.Brand.OrganizationId != _userContextService.GetOrganizationId() && !_userContextService.GetRoles().Contains("Admin"))
                return Unauthorized("Du har ikke tilladelse til at ændre denne ordre");

            if (id != order.Id)
                return BadRequest("Ids must match in URI and body");

            Order? modifiedOrder = await _orderService.UpdateObject(id, order);
            return modifiedOrder is not null ? NoContent() : NotFound("Order not found");
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Order? order = await _orderService.GetObject(id);

            if (order is not null)
                return NotFound();

            if (!_userContextService.GetRoles().Contains("Admin")) // Users cannot delete orders
                return Unauthorized("Du har ikke tilladelse til at ændre denne ordre");

            Order? deleted = await _orderService.DeleteObject(id);
            return deleted is not null ? NoContent() : NotFound("Order not found");
        }
    }
}
