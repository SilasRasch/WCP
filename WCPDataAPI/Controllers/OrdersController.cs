using Microsoft.AspNetCore.Mvc;
using WCPShared.Services;
using Microsoft.AspNetCore.Authorization;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using WCPShared.Models.DTOs;
using WCPShared.Models.UserModels;

namespace WCPDataAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly UserContextService _userContextService;
        private readonly IOrderService _orderService;
        private readonly IBrandService _brandService;
        private readonly ICreatorService _creatorService;

        public OrdersController(UserContextService userContextService, IOrderService orderService, IBrandService brandService, ICreatorService creatorService)
        {
            _userContextService = userContextService;
            _orderService = orderService;
            _brandService = brandService;
            _creatorService = creatorService;
        }


        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get([FromQuery] int? creatorId = null, [FromQuery] int? orgId = null)
        {
            var orders = await _orderService.GetAllObjects();

            if (orgId is not null) // && creatorId is null)
                return Ok(orders.Where(x => x.Brand.OrganizationId == orgId.Value));
                
            else if (_userContextService.GetRoles().Contains("Bruger")) // Catch (get by JWT role)
                return Ok(orders.Where(x => x.Brand.OrganizationId == _userContextService.GetOrganizationId()));

            if (creatorId is not null)
                return Ok(orders.Where(x => x.Creators.Any(x => x.Id == creatorId.Value)));
            else if (_userContextService.GetRoles().Contains("Creator") || _userContextService.GetRoles().Contains("Editor")) // Catch (get by JWT role)
                return Ok(orders.Where(x => x.Creators!.Any(x => x.Id == _userContextService.GetId())));

            if (_userContextService.GetRoles().Contains("Admin") && creatorId is null && orgId is null)
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
        public async Task<ActionResult<Order>> Post(OrderDto order)
        {
            if (!order.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            Brand? brand = await _brandService.GetObject(order.BrandId);
            if (brand is null)
                return BadRequest("Brand not found");

            await _orderService.AddObject(order);
            return Created();
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Order>> Put(OrderDto order, int id)
        {
            Order? existingOrder = await _orderService.GetObject(id);

            if (existingOrder is null)
                return NotFound("Order not found");

            if (!order.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            if (existingOrder.Brand.OrganizationId != _userContextService.GetOrganizationId() && !_userContextService.GetRoles().Contains("Admin"))
                return Unauthorized("Du har ikke tilladelse til at ændre denne ordre");

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
