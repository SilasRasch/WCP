using Microsoft.AspNetCore.Mvc;
using WCPShared.Services;
using Microsoft.AspNetCore.Authorization;
using WCPShared.Models.DTOs;
using WCPShared.Models.DTOs.RangeDTOs;
using WCPShared.Models.Views;
using WCPShared.Services.EntityFramework;
using WCPShared.Models.Entities;

namespace WCPDataAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly UserContextService _userContextService;
        private readonly OrderService _orderService;
        private readonly BrandService _brandService;
        private readonly CreatorService _creatorService;

        public OrdersController(UserContextService userContextService, OrderService orderService, BrandService brandService, CreatorService creatorService)
        {
            _userContextService = userContextService;
            _orderService = orderService;
            _brandService = brandService;
            _creatorService = creatorService;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderView>>> Get([FromQuery] int status, [FromQuery] int? statusTo = null, [FromQuery] int? userId = null, [FromQuery] int? orgId = null)
        {
            IEnumerable<OrderView> orders = new List<OrderView>();

            // Get user's projects
            if (_userContextService.GetRoles().Contains("Bruger"))
                orders = await _orderService.GetObjectsViewBy(x => x.Brand.OrganizationId == _userContextService.GetOrganizationId());

            // Get creator's projects
            else if (_userContextService.GetRoles().Contains("Creator"))
                orders = await _orderService.GetObjectsViewBy(x => 
                (x.Creators!.Any(x => x.UserId == _userContextService.GetId()) || x.EditorId == _userContextService.GetId() || x.VideographerId == _userContextService.GetId())); // 

            if (_userContextService.GetRoles().Contains("Admin"))
                orders = await _orderService.GetAllObjectsView();

            // Filter by status
            if (statusTo is not null)
                orders = orders.Where(x => x.Status >= status && x.Status <= statusTo);
            else orders = orders.Where(x => x.Status == status);

            // Exit if not admin
            if (!_userContextService.GetRoles().Contains("Admin"))
                return orders.ToList();

            if (orgId is not null)
                orders = orders.Where(x => x.Brand.OrganizationId == orgId.Value);

            if (userId is not null)
                orders = orders.Where(x => x.Creators.Any(x => x.User.Id == userId.Value));

            return Ok(orders);
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderView>> Get(int id)
        {
            OrderView? order = await _orderService.GetObjectViewBy(x => x.Id == id);
            return order is not null ? Ok(order) : NotFound();
        }

        // POST api/<OrdersController>
        [HttpPost()]
        public async Task<IActionResult> Post(OrderDto order)
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
        public async Task<IActionResult> Post(OrderDtoList request)
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

        [HttpPost("CreatorDelivery")]
        public async Task<IActionResult> CreatorDelivery([FromQuery] int order, [FromQuery] int creator)
        {
            Order? existingOrder = await _orderService.GetObject(order);
            if (existingOrder is null) return NotFound("Order not found");

            var callingUser = await _creatorService.GetObjectViewBy(x => x.UserId == _userContextService.GetId());
            if (callingUser is null) return Unauthorized("You are not a creator");
            

            bool isAdmin = _userContextService.GetRoles().Contains("Admin");
            bool isCreator = _userContextService.GetRoles().Contains("Creator");
            bool creatorAllowed = existingOrder.Creators.Exists(x => x.Id == creator) && callingUser.Id == creator;

            if (!isAdmin && (isCreator && !creatorAllowed))
                return Unauthorized("Du har ikke tilladelse til at ændre denne ordre");

            await _orderService.CreatorDelivery(order, creator);
            return NoContent();
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(OrderDto order, int id)
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
