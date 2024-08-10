using Microsoft.AspNetCore.Mvc;
using PtoBackend.Models;
using PtoBackend.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PtoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalWorkflowsController : ControllerBase
    {
        private readonly ApprovalWorkflowService _approvalWorkflowService;

        public ApprovalWorkflowsController(ApprovalWorkflowService approvalWorkflowService)
        {
            _approvalWorkflowService = approvalWorkflowService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApprovalWorkflow>>> Get() => await _approvalWorkflowService.GetAsync();

        [HttpGet("{id:length(24)}", Name = "GetApprovalWorkflow")]
        public async Task<ActionResult<ApprovalWorkflow>> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("El ID no puede ser nulo o vacío.");
            }

            var approvalWorkflow = await _approvalWorkflowService.GetAsync(id);

            return approvalWorkflow == null ? NotFound() : approvalWorkflow;
        }

[HttpPost]
public async Task<ActionResult<ApprovalWorkflow>> Create(ApprovalWorkflow approvalWorkflow)
{
    if (approvalWorkflow == null)
    {
        return BadRequest("El objeto de flujo de trabajo de aprobación no puede ser nulo.");
    }

    await _approvalWorkflowService.CreateAsync(approvalWorkflow);
    
    // Comprobamos si Id es nulo antes de usarlo
    if (approvalWorkflow.Id == null)
    {
        return CreatedAtAction(nameof(Get), new { id = "unknown" }, approvalWorkflow);
    }
    
    return CreatedAtRoute("GetApprovalWorkflow", new { id = approvalWorkflow.Id.ToString() }, approvalWorkflow);
}


        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, ApprovalWorkflow approvalWorkflowIn)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("El ID no puede ser nulo o vacío.");
            }

            if (approvalWorkflowIn == null)
            {
                return BadRequest("El objeto de flujo de trabajo de aprobación no puede ser nulo.");
            }

            var approvalWorkflow = await _approvalWorkflowService.GetAsync(id);

            if (approvalWorkflow == null)
            {
                return NotFound();
            }

            await _approvalWorkflowService.UpdateAsync(id, approvalWorkflowIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("El ID no puede ser nulo o vacío.");
            }

            var approvalWorkflow = await _approvalWorkflowService.GetAsync(id);

            if (approvalWorkflow == null)
            {
                return NotFound();
            }

            await _approvalWorkflowService.RemoveAsync(id);
            return NoContent();
        }
    }
}
