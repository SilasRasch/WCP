using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WCPShared.Interfaces;
using WCPShared.Models;

namespace WCPDataAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationsController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organization>>> Get()
        {
            return Ok(await _organizationService.GetAllObjects());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Organization>> Get(int id)
        {
            Organization? organization = await _organizationService.GetObject(id);
            return organization is null ? NotFound() : Ok(organization);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(int id, [FromBody] Organization organization)
        {
            if (!organization.Validate()) return BadRequest("Valideringsfejl");

            // Check if element already exists
            if (_organizationService.GetAllObjects().Result.Any(x => x.CVR == organization.CVR))
                return BadRequest("Der eksisterer allerede en organisation med det CVR");

            await _organizationService.AddObject(organization);
            return Created();
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] Organization organization)
        {
            Organization? oldOrg = await _organizationService.GetObject(id);

            if (oldOrg is null) return NotFound();
            if (!organization.Validate() || id != organization.Id) return BadRequest("Valideringsfejl");

            await _organizationService.UpdateObject(id, organization);
            return Ok();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Organization? organization = await _organizationService.GetObject(id);
            if (organization is null) return NotFound();

            await _organizationService.DeleteObject(id);
            return Ok();
        }
    }
}
