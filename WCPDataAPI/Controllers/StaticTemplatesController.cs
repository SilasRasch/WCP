using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WCPShared.Models.DTOs;
using WCPShared.Models.Entities;
using WCPShared.Models.Views;
using WCPShared.Services.EntityFramework;

namespace WCPDataAPI.Controllers
{
    [Authorize]
    [Route("api/Templates")]
    [ApiController]
    public class StaticTemplatesController : ControllerBase
    {
        private readonly StaticTemplateService _templateService;

        public StaticTemplatesController(StaticTemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaticTemplateView>>> Get()
        {
            return Ok(await _templateService.GetAllObjectsView());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StaticTemplateView>> Get(int id)
        {
            StaticTemplateView? template = await _templateService.GetObjectViewBy(x => x.Id == id);
            return template is null ? NotFound() : Ok(template);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] StaticTemplateDto dto)
        {
            if (!dto.Validate()) return BadRequest("Valideringsfejl");

            await _templateService.AddObject(dto);
            return Created();
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] StaticTemplateDto dto)
        {
            StaticTemplate? existingTemplate = await _templateService.GetObject(id);

            if (existingTemplate is null) return NotFound();
            if (!dto.Validate()) return BadRequest("Valideringsfejl");

            await _templateService.UpdateObject(id, dto);
            return Ok();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            StaticTemplate? template = await _templateService.GetObject(id);
            if (template is null) return NotFound();

            await _templateService.DeleteObject(id);
            return Ok();
        }
    }
}
