using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using PtoBackend.Models;
using PtoBackend.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace PtoBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PtoRequestsController : ControllerBase
    {
        private readonly PtoRequestService _ptoRequestService;
        private readonly ILogger<PtoRequestsController> _logger;

        public PtoRequestsController(PtoRequestService ptoRequestService, ILogger<PtoRequestsController> logger)
        {
            _ptoRequestService = ptoRequestService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<PtoRequest>>> Get() 
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuario no autenticado");
            }
            _logger.LogInformation($"Obteniendo solicitudes de PTO para el usuario: {userId}");
            return await _ptoRequestService.GetAsyncForUser(userId);
        }

        [HttpGet("available-days")]
        public async Task<ActionResult<int>> GetAvailableDays()
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuario no autenticado");
            }
            _logger.LogInformation($"Obteniendo días disponibles para el usuario: {userId}");
            return await _ptoRequestService.GetAvailableDaysAsync(userId);
        }

        [HttpGet("{id:length(24)}", Name = "GetPtoRequest")]
        public async Task<ActionResult<PtoRequest>> Get(string id)
        {
            _logger.LogInformation($"Obteniendo solicitud de PTO con ID: {id}");
            var ptoRequest = await _ptoRequestService.GetAsync(id);

            if (ptoRequest == null)
            {
                _logger.LogWarning($"Solicitud de PTO no encontrada: {id}");
                return NotFound();
            }

            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId) || ptoRequest.UserId != userId)
            {
                _logger.LogWarning($"Acceso no autorizado a la solicitud de PTO: {id}");
                return Forbid();
            }

            return ptoRequest;
        }

        [HttpPost]
        public async Task<ActionResult<PtoRequest>> Create(PtoRequestDto ptoRequestDto)
        {
            _logger.LogInformation($"Recibida solicitud de PTO: {JsonSerializer.Serialize(ptoRequestDto)}");

            if (ptoRequestDto == null)
            {
                _logger.LogWarning("Intento de crear una solicitud de PTO nula");
                return BadRequest("El objeto de solicitud de PTO no puede ser nulo.");
            }

            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuario no autenticado");
            }

            if (ptoRequestDto.StartDate >= ptoRequestDto.EndDate)
            {
                _logger.LogWarning($"Intento de crear una solicitud de PTO con fechas inválidas: StartDate={ptoRequestDto.StartDate}, EndDate={ptoRequestDto.EndDate}");
                return BadRequest("La fecha de inicio debe ser anterior a la fecha de fin.");
            }

            var availableDays = await _ptoRequestService.GetAvailableDaysAsync(userId);
            var requestedDays = (ptoRequestDto.EndDate - ptoRequestDto.StartDate).Days + 1;

            _logger.LogInformation($"Días solicitados: {requestedDays}, Días disponibles: {availableDays}");

            if (requestedDays > availableDays)
            {
                _logger.LogWarning($"Intento de solicitar más días de los disponibles. Solicitados: {requestedDays}, Disponibles: {availableDays}");
                return BadRequest($"No tienes suficientes días disponibles para esta solicitud. Días solicitados: {requestedDays}, Días disponibles: {availableDays}");
            }

            var ptoRequest = new PtoRequest
            {
                UserId = userId,
                StartDate = ptoRequestDto.StartDate,
                EndDate = ptoRequestDto.EndDate,
                Status = "Pendiente"
            };

            _logger.LogInformation($"Creando nueva solicitud de PTO para el usuario: {userId}");
            await _ptoRequestService.CreateAsync(ptoRequest);
            
            return CreatedAtRoute("GetPtoRequest", new { id = ptoRequest.Id?.ToString() ?? "unknown" }, ptoRequest);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, PtoRequest ptoRequestIn)
        {
            _logger.LogInformation($"Actualizando solicitud de PTO con ID: {id}");
            var ptoRequest = await _ptoRequestService.GetAsync(id);

            if (ptoRequest == null)
            {
                _logger.LogWarning($"Intento de actualizar una solicitud de PTO no existente: {id}");
                return NotFound();
            }

            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId) || ptoRequest.UserId != userId)
            {
                _logger.LogWarning($"Acceso no autorizado al intentar actualizar la solicitud de PTO: {id}");
                return Forbid();
            }

            await _ptoRequestService.UpdateAsync(id, ptoRequestIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation($"Eliminando solicitud de PTO con ID: {id}");
            var ptoRequest = await _ptoRequestService.GetAsync(id);

            if (ptoRequest == null)
            {
                _logger.LogWarning($"Intento de eliminar una solicitud de PTO no existente: {id}");
                return NotFound();
            }

            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId) || ptoRequest.UserId != userId)
            {
                _logger.LogWarning($"Acceso no autorizado al intentar eliminar la solicitud de PTO: {id}");
                return Forbid();
            }

            await _ptoRequestService.RemoveAsync(id);
            return NoContent();
        }
    }
}
