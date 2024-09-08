using Microsoft.AspNetCore.Mvc;
using WCPShared.Services;
using Microsoft.AspNetCore.Authorization;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using WCPShared.Models.DTOs;
using WCPShared.Models.UserModels;
using WCPShared.Models.DTOs.RangeDTOs;

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
        public async Task<ActionResult<IEnumerable<Order>>> Get([FromQuery] int? userId = null, [FromQuery] int? orgId = null)
        {
            var orders = await _orderService.GetAllObjects();

            if (orgId is not null) // && creatorId is null)
                return Ok(orders.Where(x => x.Brand.OrganizationId == orgId.Value));
                
            else if (_userContextService.GetRoles().Contains("Bruger")) // Catch (get by JWT role)
                return Ok(orders.Where(x => x.Brand.OrganizationId == _userContextService.GetOrganizationId()));

            if (userId is not null)
                return Ok(orders.Where(x => x.Creators.Any(x => x.UserId == userId.Value)));
            else if (_userContextService.GetRoles().Contains("Creator") || _userContextService.GetRoles().Contains("Editor")) // Catch (get by JWT role)
                return Ok(orders.Where(x => x.Creators!.Any(x => x.UserId == _userContextService.GetId())));

            if (_userContextService.GetRoles().Contains("Admin") && userId is null && orgId is null)
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
        [HttpPost()]
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

        [HttpPost("AddRange"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Order>> Post(OrderDtoList request)
        {
            foreach (OrderDto order in request.Orders)
            {
                if (!order.Validate())
                    return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

                Brand? brand = await _brandService.GetObject(order.BrandId);
                if (brand is null)
                    return BadRequest("Brand not found");

                await _orderService.AddObject(order);
            }
            
            
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

            bool creatorNotAllowed = !existingOrder.Creators.Exists(x => x.Id == _userContextService.GetId()) && existingOrder.Status == 3 && order.Status == 4;
            bool isAdmin = _userContextService.GetRoles().Contains("Admin");
            bool isCreator = _userContextService.GetRoles().Contains("Creator");
            bool isUser = _userContextService.GetRoles().Contains("Bruger");
            //bool isNotInOrg = existingOrder.Brand.OrganizationId != _userContextService.GetOrganizationId();

            if (!isAdmin && (isCreator && creatorNotAllowed) && (isUser && existingOrder.Brand.OrganizationId != _userContextService.GetOrganizationId()))
                return Unauthorized("Du har ikke tilladelse til at ændre denne ordre");

            Order? modifiedOrder = await _orderService.UpdateObject(id, order);
            return modifiedOrder is not null ? NoContent() : NotFound("Order not found");
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Order? order = await _orderService.GetObject(id);

            if (order is null)
                return NotFound();

            if (!_userContextService.GetRoles().Contains("Admin")) // Users cannot delete orders
                return Unauthorized("Du har ikke tilladelse til at ændre denne ordre");

            Order? deleted = await _orderService.DeleteObject(id);
            return deleted is not null ? NoContent() : NotFound("Order not found");
        }
    }
}
