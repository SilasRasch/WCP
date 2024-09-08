using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WCPShared.Services;
using WCPShared.Models;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models.DTOs.RangeDTOs;
using WCPShared.Models.Views;

namespace WCPDataAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IOrganizationService _organizationService;
        private readonly UserContextService _userContextService;

        public BrandsController(IBrandService brandService, UserContextService userContextService, IOrganizationService organizationService)
        {
            _userContextService = userContextService;
            _brandService = brandService;
            _organizationService = organizationService;
        }

        // GET: api/<BrandsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandView>>> Get([FromQuery] int? orgId = null)
        {
            if (_userContextService.GetRoles().Contains("Bruger"))
                return await _brandService.GetObjectsViewBy(x => x.OrganizationId == _userContextService.GetOrganizationId());
            
            if (orgId is not null)
                return await _brandService.GetObjectsViewBy(x => x.OrganizationId == orgId);

            if (_userContextService.GetRoles().Contains("Bruger"))
                return await _brandService.GetAllObjectsView();
            
            return NotFound("Ingen brands at finde...");
        }

        // GET api/<BrandsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BrandView>> Get(int id)
        {
            BrandView? brand = await _brandService.GetObjectViewBy(x => x.Id == id);
            return brand is not null ? Ok(brand) : NotFound("Der blev ikke fundet nogen brand med det id...");
        }

        // POST api/<BrandsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BrandDto request)
        {
            var org = await _organizationService.GetObject(request.OrganizationId);
            
            if (org is null)
                return BadRequest("Organization doesn't exist");
            
            if (!request.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            await _brandService.AddObject(request);
            return Created();
        }

        [HttpPost("AddRange"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostRange([FromBody] BrandDtoList request)
        {
            foreach (var brand in request.Brands)
            {
                var org = await _organizationService.GetObject(brand.OrganizationId);

                if (org is null)
                    return BadRequest("Organization doesn't exist");

                if (!brand.Validate())
                    return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

                await _brandService.AddObject(brand);
            }

            return Created();
        }

        // PUT api/<BrandsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BrandDto brand)
        {
            if (!brand.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            if (!_userContextService.GetRoles().Contains("Admin") && brand.OrganizationId != _userContextService.GetOrganizationId())
                return Unauthorized("You are not the owner of this brand");

            Brand? modifiedBrand = await _brandService.UpdateObject(id, brand);
            return modifiedBrand is not null ? NoContent() : NotFound("Brand not found");
        }

        // DELETE api/<BrandsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Brand? brand = await _brandService.GetObject(id);

            if (brand is null)
                return BadRequest("Brand not found");

            if (!_userContextService.GetRoles().Contains("Admin") && brand.OrganizationId != _userContextService.GetOrganizationId())
                return BadRequest("You are not the owner of this brand");

            Brand? deleted = await _brandService.DeleteObject(id);
            return deleted is not null ? NoContent() : NotFound("Brand not found");
        }
    }
}
