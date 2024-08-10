using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace PtoBackend.Models
{
    public class ApprovalWorkflow
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // Puede ser nulo

        [BsonElement("requestId")]
        public string RequestId { get; set; } = string.Empty; // Asignar un valor predeterminado

        [BsonElement("approverId")]
        public string ApproverId { get; set; } = string.Empty; // Asignar un valor predeterminado

        [BsonElement("status")]
        public string Status { get; set; } = string.Empty; // Asignar un valor predeterminado

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Asignar valor por defecto

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Asignar valor por defecto
    }
}
