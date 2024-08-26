using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WCPShared.Interfaces;
using WCPShared.Models.BrandModels;
using WCPShared.Services;
using WCPShared.Services.StaticHelpers;
using WCPShared.Models;
using Azure.Core;

namespace WCPDataAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly UserContextService _userContextService;

        public BrandsController(IBrandService brandService, UserContextService userContextService)
        {
            _userContextService = userContextService;
            _brandService = brandService;
        }

        // GET: api/<BrandsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> Get([FromQuery] int? orgId = null)
        {
            IEnumerable<Brand> brands;

            if (orgId is not null)
                brands = await _brandService.GetAllObjects(x => x.OrganizationId == orgId);
            else if (_userContextService.GetRoles().Contains("Bruger")) // Catch (get by JWT role)
                brands = await _brandService.GetAllObjects(x => x.OrganizationId == _userContextService.GetOrganizationId());
            else if (_userContextService.GetRoles().Contains("Admin"))
                brands = await _brandService.GetAllObjects();
            else
                brands = new List<Brand>();

            return brands is not null ? Ok(brands) : NotFound("Ingen brands at finde...");
        }

        // GET api/<BrandsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> Get(int id)
        {
            Brand? brand = await _brandService.GetObject(id);
            return brand is not null ? Ok(brand) : NotFound("Der blev ikke fundet nogen brand med det id...");
        }

        // POST api/<BrandsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BrandDto request)
        {
            if (!request.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            await _brandService.AddObject(new Brand
            {
                Name = request.Name,
                OrganizationId = request.OrganizationId,
                URL = request.URL,
            });

            return Created();
        }

        // PUT api/<BrandsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Brand brand)
        {
            if (!brand.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            if (id != brand.Id || (brand.OrganizationId != _userContextService.GetOrganizationId() && !_userContextService.GetRoles().Contains("Admin")))
                throw new Exception("You are not the owner of this brand");

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

            if (brand.OrganizationId != _userContextService.GetOrganizationId() && !_userContextService.GetRoles().Contains("Admin"))
                return BadRequest("You are not the owner of this brand");

            Brand? deleted = await _brandService.DeleteObject(id);
            return deleted is not null ? NoContent() : NotFound("Brand not found");
        }
    }
}
