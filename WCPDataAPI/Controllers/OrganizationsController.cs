using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using WCPShared.Models.DTOs;
using WCPShared.Models.Views;

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
        public async Task<ActionResult<IEnumerable<OrganizationView>>> Get()
        {
            return Ok(await _organizationService.GetAllObjectsView());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizationView>> Get(int id)
        {
            OrganizationView? organization = await _organizationService.GetObjectViewBy(x => x.Id == id);
            return organization is null ? NotFound() : Ok(organization);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] OrganizationDto organization)
        {
            if (!organization.Validate()) return BadRequest("Valideringsfejl");

            // Check if element already exists
            if ((await _organizationService.GetAllObjects()).Any(x => x.CVR == organization.CVR))
                return BadRequest("Der eksisterer allerede en organisation med det CVR");

            var result = await _organizationService.AddObject(organization);
            return result is not null ? Ok(result.Id) : BadRequest();
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] OrganizationDto organization)
        {
            Organization? oldOrg = await _organizationService.GetObject(id);

            if (oldOrg is null) return NotFound();
            if (!organization.Validate()) return BadRequest("Valideringsfejl");

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
