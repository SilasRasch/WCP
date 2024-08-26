using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WCPShared.Interfaces;
using WCPShared.Models.BrandModels;
using WCPShared.Services.Databases;
using WCPShared.Services;
using WCPShared.Services.StaticHelpers;

namespace WCPDataAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly UserContextService _userContextService;
        private readonly IMongoCollection<Brand> _brands;

        public BrandsController(MongoDbService mongoDbService, IEmailService emailService, UserContextService userContextService)
        {
            _brands = mongoDbService.Database?.GetCollection<Brand>(Secrets.MongoBrandCollectionName)!;
            _emailService = emailService;
            _userContextService = userContextService;
        }

        // GET: api/<BrandsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> Get([FromQuery] int? orgId = null)
        {
            IEnumerable<Brand> brands;

            if (orgId is not null)
                brands = await _brands.FindAsync(x => x.OrganizationId == orgId).Result.ToListAsync();
            else if (_userContextService.GetRoles().Contains("Bruger")) // Catch (get by JWT role)
                brands = await _brands.FindAsync(x => x.OrganizationId == _userContextService.GetOrganizationId()).Result.ToListAsync();
            else if (_userContextService.GetRoles().Contains("Admin"))
                brands = await _brands.Find(FilterDefinition<Brand>.Empty).ToListAsync();
            else
                brands = new List<Brand>();

            return brands is not null ? Ok(brands) : NotFound("Ingen brands at finde...");
        }

        // GET api/<BrandsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> Get(int id)
        {
            Brand? brand = await _brands.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();

            return brand is not null ? Ok(brand) : NotFound("Der blev ikke fundet nogen brand med det id...");
        }

        // POST api/<BrandsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BrandDto request)
        {
            if (!request.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            Brand lastBrand;

            if (await _brands.CountDocumentsAsync(FilterDefinition<Brand>.Empty) > 0)
                lastBrand = await _brands.Find(FilterDefinition<Brand>.Empty).SortByDescending(o => o.Id).Limit(1).FirstAsync();
            else lastBrand = null!;

            Brand brand = new Brand
            {
                Id = lastBrand != null ? lastBrand.Id + 1 : 1000,
                OrganizationId = request.OrganizationId,
                Name = request.Name,
                URL = request.URL
            };

            await _brands.InsertOneAsync(brand);
            await _emailService.SendBrandCreationEmail(brand, _userContextService.GetEmail());

            return CreatedAtAction("Post", new { id = brand.Id }, brand);
        }

        // PUT api/<BrandsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Brand brand)
        {
            Brand? oldBrand = await _brands.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (oldBrand is null) return NotFound();

            if (id != brand.Id || (brand.OrganizationId != _userContextService.GetOrganizationId() && !_userContextService.GetRoles().Contains("Admin")))
                return BadRequest("You are not the owner of this brand");

            if (!brand.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            ReplaceOneResult result = await _brands.ReplaceOneAsync(x => x.Id == brand.Id, brand);

            if (result.IsAcknowledged)
                return result.ModifiedCount == 1 ? Ok(result) : NotFound();

            return NotFound();
        }

        // DELETE api/<BrandsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Brand? brand = await _brands.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (brand is null) return NotFound();
            if (brand.OrganizationId != _userContextService.GetOrganizationId() && !_userContextService.GetRoles().Contains("Admin"))
                return BadRequest("You are not the owner of this brand");

            DeleteResult result = await _brands.DeleteOneAsync(x => x.Id == id);

            if (result.IsAcknowledged)
                return result.DeletedCount == 1 ? NoContent() : NotFound();

            return NotFound();
        }
    }
}
