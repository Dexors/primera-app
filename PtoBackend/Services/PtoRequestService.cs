using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using PtoBackend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PtoBackend.Services
{
    public class PtoRequestService
    {
        private readonly IMongoCollection<PtoRequest> _ptoRequests;
        private readonly ILogger<PtoRequestService> _logger;
        private readonly int _totalDaysPerYear;

        public PtoRequestService(IConfiguration configuration, ILogger<PtoRequestService> logger)
        {
            _logger = logger;
            
            var mongoDbConnectionString = configuration.GetConnectionString("MongoDbConnection");
            var client = new MongoClient(mongoDbConnectionString);
            var database = client.GetDatabase("PtoDb");
            _ptoRequests = database.GetCollection<PtoRequest>("PtoRequests");
            
            _totalDaysPerYear = configuration.GetValue<int>("PtoSettings:TotalDaysPerYear", 20);
        }

        public async Task<List<PtoRequest>> GetAsyncForUser(string userId)
        {
            _logger.LogInformation($"Obteniendo solicitudes de PTO para el usuario: {userId}");
            return await _ptoRequests.Find(ptoRequest => ptoRequest.UserId == userId).ToListAsync();
        }

        public async Task<PtoRequest> GetAsync(string id)
        {
            _logger.LogInformation($"Obteniendo solicitud de PTO con ID: {id}");
            return await _ptoRequests.Find<PtoRequest>(ptoRequest => ptoRequest.Id == id).FirstOrDefaultAsync();
        }

        public async Task<PtoRequest> CreateAsync(PtoRequest ptoRequest)
        {
            _logger.LogInformation($"Creando nueva solicitud de PTO para el usuario: {ptoRequest.UserId}");
            await _ptoRequests.InsertOneAsync(ptoRequest);
            return ptoRequest;
        }

        public async Task UpdateAsync(string id, PtoRequest ptoRequestIn)
        {
            _logger.LogInformation($"Actualizando solicitud de PTO con ID: {id}");
            ptoRequestIn.UpdatedAt = DateTime.UtcNow;
            await _ptoRequests.ReplaceOneAsync(ptoRequest => ptoRequest.Id == id, ptoRequestIn);
        }

        public async Task RemoveAsync(string id)
        {
            _logger.LogInformation($"Eliminando solicitud de PTO con ID: {id}");
            await _ptoRequests.DeleteOneAsync(ptoRequest => ptoRequest.Id == id);
        }

        public async Task<int> GetAvailableDaysAsync(string userId)
        {
            _logger.LogInformation($"Calculando días disponibles para el usuario: {userId}");
            var usedRequests = await _ptoRequests
                .Find(pr => pr.UserId == userId && pr.Status == "Aprobado" && pr.StartDate.Year == DateTime.UtcNow.Year)
                .ToListAsync();
            
            int usedDaysCount = usedRequests.Sum(pr => (pr.EndDate - pr.StartDate).Days + 1);
            int availableDays = Math.Max(0, _totalDaysPerYear - usedDaysCount);
            
            _logger.LogInformation($"Días disponibles para el usuario {userId}: {availableDays}");
            return availableDays;
        }
    }
}
