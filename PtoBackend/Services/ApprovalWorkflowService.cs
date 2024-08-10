using MongoDB.Driver;
using PtoBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PtoBackend.Services
{
    public class ApprovalWorkflowService
    {
        private readonly IMongoCollection<ApprovalWorkflow> _approvalWorkflows;
        private readonly IMongoClient _client; // Declarar la variable como campo de la clase

        public ApprovalWorkflowService()
        {
            var connectionString = "mongodb://localhost:27017"; // Nota: El nombre de la base de datos en la cadena de conexión no es necesario si ya se especifica en GetDatabase
            _client = new MongoClient(connectionString); 
            var database = _client.GetDatabase("PtoDb"); // Nombre de la base de datos
            _approvalWorkflows = database.GetCollection<ApprovalWorkflow>("ApprovalWorkflows");
        }

        public async Task<List<ApprovalWorkflow>> GetAsync() => await _approvalWorkflows.Find(approvalWorkflow => true).ToListAsync();

        public async Task<ApprovalWorkflow> GetAsync(string id) => await _approvalWorkflows.Find<ApprovalWorkflow>(approvalWorkflow => approvalWorkflow.Id == id).FirstOrDefaultAsync();

        public async Task<ApprovalWorkflow> CreateAsync(ApprovalWorkflow approvalWorkflow)
        {
            await _approvalWorkflows.InsertOneAsync(approvalWorkflow);
            return approvalWorkflow;
        }

        public async Task UpdateAsync(string id, ApprovalWorkflow approvalWorkflowIn) => await _approvalWorkflows.ReplaceOneAsync(approvalWorkflow => approvalWorkflow.Id == id, approvalWorkflowIn);

        public async Task RemoveAsync(string id) => await _approvalWorkflows.DeleteOneAsync(approvalWorkflow => approvalWorkflow.Id == id);
    }
}
